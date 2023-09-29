using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Controllers;
using MovieRentalApi.Models;

namespace MovieRentalApiTests.Controllers;

public class MovieControllerTests
{
	[Fact]
	public void MovieController_WhenReceivedMovie_ShouldReturnOk()
	{
		//Arrange
		var expectedResponse = new OkResult();
        var movie = new MovieCreateModel();
		
		//Act
        var controller = new MovieController();
        var response = controller.Post(movie);

		//Assert
		Assert.Equivalent(expectedResponse, response);
	}
}