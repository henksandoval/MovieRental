using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Controllers;

namespace MovieRentalApiTests.Controllers;

public class MovieControllerTests
{
	[Fact]
	public void MovieController_WhenReceivedMovie_ShouldReturnOk()
	{
		//Arrange
		var expectedResponse = new OkResult();
		
		//Act
        var controller = new MovieController();
        var response = controller.Post();

		//Assert
		Assert.Equivalent(expectedResponse, response);
	}
}