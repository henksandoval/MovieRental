﻿using AutoMapper;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore.Storage;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Exceptions;
using MovieRentalApi.Mappers;
using MovieRentalApi.Models;
using MovieRentalApi.Services;
using MovieRentalApi.Utilities;
using NSubstitute.ReturnsExtensions;

namespace MovieRentalApiTests.Services;

public class MovieRentalServiceTests
{
	private readonly IClock clock;
	private readonly IBaseRepository<MovieEntity> repositoryMovie;
	private readonly IBaseRepository<RentalEntity> repositoryRental;
	private readonly RentalService service;

	public MovieRentalServiceTests()
	{
		var configuration = new MapperConfiguration(config => { config.AddProfile<MapperProfile>(); });

		IMapper mapper = new Mapper(configuration);
		repositoryMovie = Substitute.For<IBaseRepository<MovieEntity>>();
		repositoryRental = Substitute.For<IBaseRepository<RentalEntity>>();
		clock = Substitute.For<IClock>();
		service = new RentalService(repositoryMovie, repositoryRental, clock, mapper);
	}

	[Fact(DisplayName =
		"MovieRentalService Cuando Solicitan Un Id de Pelicula Disponible, debe responder la pelicula.")]
	public async Task MovieRentalService_WhenReceiveRequestIdMovieAndMovieIsAvailable_ShouldReturnTheMovie()
	{
		//Arrange (Preparar)
		const int idMovie = 1;
		var entity = new MovieEntity { IsAvailable = true };
		repositoryMovie.GetByIdAsync(idMovie).Returns(entity);

		//Act (Actuar)
		var movieResponse = await service.RentalMovieAsync(idMovie);

		//Assert (Asegurar)
		var expectedMovie = new MovieModel();
		movieResponse.Should().BeEquivalentTo(expectedMovie);
	}

	[Fact(DisplayName =
		"MovieRentalService Cuando Solicitan una Pelicula que no existe en Database, debe arrojar (throw) una exception MovieNotFoundException.")]
	public async Task MovieRentalService_WhenRepositoryReturnNullMovieEntity_ShouldThrowException()
	{
		//Arrange (Preparar)
		const int idMovie = 1;
		repositoryMovie.GetByIdAsync(idMovie).ReturnsNull();

		//Act (Actuar)
		var action = () => service.RentalMovieAsync(idMovie);

		//Assert (Asegurar)
		await action.Should()
			.ThrowAsync<MovieNotFoundException>()
			.WithMessage($"The Movie {idMovie} is not available.");
	}

	[Fact(DisplayName =
		"MovieRentalService Cuando La Pelicula Esta Disponible, Debe Registrar en Base de datos que esta alquilada.")]
	public async Task MovieRentalService_WhenMovieIsAvailable_ShouldSaveInDatabase()
	{
		//Arrange (Preparar)
		const int idMovie = 1;
		var entity = new MovieEntity { IsAvailable = true };
		repositoryMovie.GetByIdAsync(idMovie).Returns(entity);
		var expectedDate = new DateTime(1995, 05, 16, 0, 0, 0, DateTimeKind.Utc);
		clock.GetCurrentTime().Returns(expectedDate);

		//Act (Actuar)
		_ = await service.RentalMovieAsync(idMovie);

		//Assert (Asegurar)
		using (new AssertionScope())
		{
			await repositoryMovie.Received(1)
				.UpdateAsync(Arg.Is<MovieEntity>(e => !e.IsAvailable));
			await repositoryRental.Received(1)
				.CreateAsync(Arg.Is<RentalEntity>(r => r.RentalDate == expectedDate));
			await repositoryMovie.Received(1)
				.CommitAsync(Arg.Any<IDbContextTransaction>());
		}
	}
}