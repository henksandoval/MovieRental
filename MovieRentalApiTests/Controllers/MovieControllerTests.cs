using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Controllers;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Models;
using Newtonsoft.Json;
using NSubstitute;

namespace MovieRentalApiTests.Controllers;

public class MovieControllerTests
{
	[Fact]
	public async Task MovieController_WhenReceivedMovie_ShouldReturnOk()
	{
		//Arrange
		var expectedResponse = new OkResult();
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

        var repository = Substitute.For<IBaseRepository<MovieEntity>>();
        repository.CreateAsync(Arg.Any<MovieEntity>()).Returns(movieExpected);

        //Act
        var controller = new MovieController(repository);
        var response = await controller.PostAsync(movie);

		//Assert
		var okResult = response as OkObjectResult;
        var movieResponse = okResult.Value as MovieEntity;

		Assert.Equivalent(expectedResponse, response);
		Assert.Equivalent(movieExpected, movieResponse);
	}
}