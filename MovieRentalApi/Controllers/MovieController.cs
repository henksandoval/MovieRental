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
    private readonly IMapper _mapper;

    public MovieController(IBaseRepository<MovieEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] MovieCreateModel movieCreateModel)
    {
        var movieEntity = _mapper.Map<MovieEntity>(movieCreateModel);

        movieEntity = await _repository.CreateAsync(movieEntity);

        var movieResponse = _mapper.Map<MovieModel>(movieEntity);

        return CreatedAtRoute(nameof(GetAsync), new { id = movieResponse.Id }, movieResponse);
    }

    [HttpGet("{id}", Name = "GetAsync")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var movie = await _repository.GetByIdAsync(id);
        var movieModel = _mapper.Map<MovieEntity, MovieModel>(movie);

        return Ok(movieModel);
    }
}