using MovieRentalApi.Services;

namespace MovieRentalApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Requests;

public class RentalController : ControllerBase
{
	private readonly IMovieRentalService rentalService;
	private readonly ILogger<RentalController> logger;

	public RentalController(IMovieRentalService rentalService, ILogger<RentalController> logger)
	{
		this.rentalService = rentalService;
		this.logger = logger;
	}

	public async Task<IActionResult> CreateRental([FromBody] CreateRentalRequest request)
	{
		var movie = await rentalService.RentalMovieAsync(request.MovieId);
		return Ok(movie);
	}
}