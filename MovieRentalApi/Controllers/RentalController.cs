using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Requests;
using MovieRentalApi.Services;

namespace MovieRentalApi.Controllers;

public class RentalController : ControllerBase
{
	private readonly ILogger<RentalController> logger;
	private readonly IMovieRentalService rentalService;

	public RentalController(IMovieRentalService rentalService, ILogger<RentalController> logger)
	{
		this.rentalService = rentalService;
		this.logger = logger;
	}

	public async Task<IActionResult> CreateRental([FromBody] CreateRentalRequest request)
	{
		logger.LogInformation("Request");
		var movie = await rentalService.RentalMovieAsync(request.MovieId);
		return Ok(movie);
	}
}