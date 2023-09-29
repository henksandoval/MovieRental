using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Models;

namespace MovieRentalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(MovieCreateModel movieCreateModel)
    {
        return Ok();
    }
}