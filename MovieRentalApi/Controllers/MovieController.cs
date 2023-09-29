using Microsoft.AspNetCore.Mvc;

namespace MovieRentalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    [HttpPost]
    public IActionResult Post()
    {
        return Ok();
    }
}