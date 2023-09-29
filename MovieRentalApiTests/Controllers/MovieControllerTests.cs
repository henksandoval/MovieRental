using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Controllers;
using MovieRentalApi.Entities;
using MovieRentalApi.Models;
using Newtonsoft.Json;

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
		
		//Act
        var controller = new MovieController();
        var response = controller.Post(movie);

		//Assert
		var okResult = response as OkObjectResult;
        var movieResponse = okResult.Value as MovieEntity;

		Assert.Equivalent(expectedResponse, response);
		Assert.Equivalent(movieExpected, movieResponse);
	}
}