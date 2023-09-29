using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Models;

namespace MovieRentalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly IBaseRepository<MovieEntity> _repository;

    public MovieController(IBaseRepository<MovieEntity> repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] MovieCreateModel movieCreateModel)
    {
        var movieEntity = new MovieEntity
        {
            Title = movieCreateModel.Title,
            Description = movieCreateModel.Description,
            Year = movieCreateModel.Year,
        };

        movieEntity = await _repository.CreateAsync(movieEntity);

        return CreatedAtRoute(nameof(GetAsync), new { id = movieEntity.Id }, movieEntity);
    }

    [HttpGet("{id}", Name = "GetAsync")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var movie = await _repository.GetByIdAsync(id);

        var movieModel = new MovieModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Year = movie.Year
        };

        return Ok(movieModel);
    }
}