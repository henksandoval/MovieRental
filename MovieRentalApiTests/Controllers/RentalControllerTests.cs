using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieRentalApi.Controllers;
using MovieRentalApi.Models;
using MovieRentalApi.Requests;
using MovieRentalApi.Services;

namespace MovieRentalApiTests.Controllers;

public class RentalControllerTests
{
	[Fact]
	public async Task RentalController_WhenRequestMovieById_ShouldReturnMovie()
	{
		const int expectedStatusCode = (int)HttpStatusCode.OK;
		var request = new CreateRentalRequest();
		var expectedMovie = new MovieModel();
		var rentalService = Substitute.For<IMovieRentalService>();
		rentalService.RentalMovieAsync(request.MovieId).Returns(expectedMovie);
		var controller = new RentalController(rentalService, Substitute.For<ILogger<RentalController>>());

		var result = await controller.CreateRental(request);
		var response = result as OkObjectResult;

		var movieResponse = response?.Value as MovieModel;
		response?.StatusCode.Should().Be(expectedStatusCode);
		movieResponse.Should().BeEquivalentTo(expectedMovie);
	}
}