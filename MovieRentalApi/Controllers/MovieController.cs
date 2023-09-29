using AutoMapper;
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
    private readonly IMapper _mapperProfile;

    public MovieController(IBaseRepository<MovieEntity> repository, IMapper mapperProfile)
    {
        _repository = repository;
        _mapperProfile = mapperProfile;
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
        var movieModel = _mapperProfile.Map<MovieEntity, MovieModel>(movie);

        return Ok(movieModel);
    }
}