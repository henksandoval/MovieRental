using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Controllers;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Mappers;
using MovieRentalApi.Models;

namespace MovieRentalApiTests.Controllers;

public class MovieControllerTests
{
	private readonly MovieController controller;
	private readonly Fixture fixture = new();
	private readonly IMapper mapper;
	private readonly IBaseRepository<MovieEntity> repository;

	public MovieControllerTests()
	{
		var configuration = new MapperConfiguration(config => { config.AddProfile<MapperProfile>(); });

		mapper = new Mapper(configuration);
		repository = Substitute.For<IBaseRepository<MovieEntity>>();
		controller = new MovieController(repository, mapper);
	}

	[Fact]
	public async Task MovieController_WhenReceivedMovie_ShouldReturnOk()
	{
		//Arrange
		var movieCreateModel = fixture.Create<MovieCreateModel>();
		var movieEntity = mapper.Map<MovieEntity>(movieCreateModel);

		//Act
		var response = await controller.PostAsync(movieCreateModel);

		//Assert
		var result = response as CreatedAtRouteResult;
		var movieResponse = result?.Value as MovieModel;

		var movieExpected = mapper.Map<MovieModel>(movieEntity);
		var expectedResult =
			new CreatedAtRouteResult(nameof(MovieController.GetAsync), new { id = movieEntity.Id }, movieExpected);

		result.Should().BeEquivalentTo(expectedResult);
		movieResponse.Should().BeEquivalentTo(movieExpected);
	}

	[Fact]
	public async Task MovieController_WhenReceivedAnIdentifier_ShouldReturnAnMovieModel()
	{
		//Arrange
		fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
			.ForEach(b => fixture.Behaviors.Remove(b));
		fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

		var movieEntity = fixture.Create<MovieEntity>();

		var modelExpected = mapper.Map<MovieModel>(movieEntity);
		repository.GetByIdAsync(Arg.Any<int>()).Returns(movieEntity);

		//Act
		var response = await controller.GetAsync(movieEntity.Id);

		//Assert
		var result = response as OkObjectResult;
		var modelResponse = result?.Value as MovieModel;
		modelResponse.Should().BeEquivalentTo(modelExpected);
	}
}