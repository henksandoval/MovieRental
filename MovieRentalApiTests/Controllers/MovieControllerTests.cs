using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Controllers;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Mappers;
using MovieRentalApi.Models;
using Newtonsoft.Json;
using NSubstitute;

namespace MovieRentalApiTests.Controllers;

public class MovieControllerTests
{
    private readonly MovieController _controller;
    private readonly IBaseRepository<MovieEntity> _repository;

    public MovieControllerTests()
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<MapperProfile>();
        });
        IMapper mapper = new Mapper(configuration);

        _repository = Substitute.For<IBaseRepository<MovieEntity>>();
        _controller = new MovieController(_repository, mapper);
    }

	[Fact]
	public async Task MovieController_WhenReceivedMovie_ShouldReturnOk()
	{
		//Arrange
        var movie = new MovieCreateModel
        {
            Title = "Titanic",
            Description = "Great movie",
            Year = 1997
        };

        var movieExpected = new MovieEntity
        {
            Id = 1,
            Title = movie.Title,
            Description = movie.Description,
            Year = movie.Year
        };

		var expectedResponse = new CreatedAtRouteResult(nameof(MovieController.GetAsync), new { id = movieExpected.Id }, movieExpected);
        
        _repository.CreateAsync(Arg.Any<MovieEntity>()).Returns(movieExpected);

        //Act
        
        var response = await _controller.PostAsync(movie);

		//Assert
        var result = response as CreatedAtRouteResult;
        var movieResponse = result.Value as MovieEntity;
		Assert.Equivalent(expectedResponse, result);
        Assert.Equivalent(movieExpected, movieResponse);
	}

    [Fact]
    public async Task MovieController_WhenReceivedAnIdentifier_ShouldReturnAnMovieModel()
    {
        //Arrange
        var movieEntity = new MovieEntity();
        var modelExpected = new MovieModel();
        _repository.GetByIdAsync(Arg.Any<int>()).Returns(movieEntity);

        //Act
        var response = await _controller.GetAsync(modelExpected.Id);
        var result = response as OkObjectResult;
        var modelResponse = result.Value as MovieModel;

        //Assert
        Assert.Equivalent(modelExpected, modelResponse);
    }
}