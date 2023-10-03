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
	private readonly IMapper mapper;
	private readonly IBaseRepository<MovieEntity> repository;

	public MovieController(IBaseRepository<MovieEntity> repository, IMapper mapper)
	{
		this.repository = repository;
		this.mapper = mapper;
	}

	[HttpPost]
	public async Task<IActionResult> PostAsync([FromBody] MovieCreateModel movieCreateModel)
	{
		var movieEntity = mapper.Map<MovieEntity>(movieCreateModel);

		await repository.CreateAsync(movieEntity);

		var movieResponse = mapper.Map<MovieModel>(movieEntity);

		return CreatedAtRoute(nameof(GetAsync), new { id = movieResponse.Id }, movieResponse);
	}

	[HttpGet("{id}", Name = "GetMoviesAsync")]
	public async Task<IActionResult> GetAsync(int id)
	{
		var movie = await repository.GetByIdAsync(id);
		var movieModel = mapper.Map<MovieEntity, MovieModel>(movie);

		return Ok(movieModel);
	}
}