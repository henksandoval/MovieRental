using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Entities;
using MovieRentalApi.Models;

namespace MovieRentalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(MovieCreateModel movieCreateModel)
    {
        var movieEntity = new MovieEntity
        {
            Id = 1,
            Title = movieCreateModel.Title,
            Description = movieCreateModel.Description,
            Year = movieCreateModel.Year,
        };

        return Ok(movieEntity);
    }
}