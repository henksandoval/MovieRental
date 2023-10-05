using AutoMapper;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Exceptions;
using MovieRentalApi.Models;
using MovieRentalApi.Utilities;

namespace MovieRentalApi.Services;

public class MovieRentalService
{
	private readonly IClock clock;
	private readonly IMapper mapper;
	private readonly IBaseRepository<MovieEntity> repositoryMovie;
	private readonly IBaseRepository<RentalEntity> repositoryRental;

	public MovieRentalService(IBaseRepository<MovieEntity> repositoryMovie,
		IBaseRepository<RentalEntity> repositoryRental,
		IClock clock,
		IMapper mapper)
	{
		this.repositoryMovie = repositoryMovie;
		this.repositoryRental = repositoryRental;
		this.clock = clock;
		this.mapper = mapper;
	}

	public async Task<MovieModel> RentalMovieAsync(int idMovie)
	{
		if (await repositoryMovie.GetByIdAsync(idMovie) is not { IsAvailable: true } movieEntity)
			throw new MovieNotFoundException($"The Movie {idMovie} is not available.");

		movieEntity.IsAvailable = false;
		var movie = mapper.Map<MovieModel>(movieEntity);
		var rentalEntity = new RentalEntity { RentalDate = clock.GetCurrentTime() };

		var transaction = await repositoryMovie.BeginTransactionAsync();
		await repositoryMovie.UpdateAsync(movieEntity);
		await repositoryRental.CreateAsync(rentalEntity);
		await repositoryMovie.CommitAsync(transaction);

		return movie;
	}
}