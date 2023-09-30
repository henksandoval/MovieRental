using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Controllers;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Mappers;
using MovieRentalApi.Models;
using NSubstitute;

namespace MovieRentalApiTests.Controllers;

public class MovieControllerTests
{
    private readonly MovieController _controller;
    private readonly IBaseRepository<MovieEntity> _repository;
    private readonly Fixture _fixture = new Fixture();
    private readonly IMapper _mapper;

    public MovieControllerTests()
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<MapperProfile>();
        });

        _mapper = new Mapper(configuration);
        _repository = Substitute.For<IBaseRepository<MovieEntity>>();
        _controller = new MovieController(_repository, _mapper);
    }

	[Fact]
	public async Task MovieController_WhenReceivedMovie_ShouldReturnOk()
	{
		//Arrange
        var movieCreateModel = _fixture.Create<MovieCreateModel>();

        var movieEntity = _mapper.Map<MovieEntity>(movieCreateModel);
        movieEntity.Id = 1;
        _repository.CreateAsync(Arg.Any<MovieEntity>()).Returns(movieEntity);

        //Act
        var response = await _controller.PostAsync(movieCreateModel);

        //Assert
        var result = response as CreatedAtRouteResult;
        var movieResponse = result.Value as MovieModel;

        var movieExpected = _mapper.Map<MovieModel>(movieEntity);
		var expectedResponse = new CreatedAtRouteResult(nameof(MovieController.GetAsync), new { id = movieEntity.Id }, movieExpected);

		Assert.Equivalent(expectedResponse, result);
        Assert.Equivalent(movieExpected, movieResponse);
	}

    [Fact]
    public async Task MovieController_WhenReceivedAnIdentifier_ShouldReturnAnMovieModel()
    {
        //Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var movieEntity = _fixture.Create<MovieEntity>();

        var modelExpected = _mapper.Map<MovieModel>(movieEntity);
        _repository.GetByIdAsync(Arg.Any<int>()).Returns(movieEntity);

        //Act
        var response = await _controller.GetAsync(movieEntity.Id);

        //Assert
        var result = response as OkObjectResult;
        var modelResponse = result.Value as MovieModel;
        Assert.Equivalent(modelExpected, modelResponse);
    }
}