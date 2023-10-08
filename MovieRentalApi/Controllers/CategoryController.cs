using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Models;
using MovieRentalApi.Requests;

namespace MovieRentalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
	private readonly IMapper mapper;
	private readonly IBaseRepository<CategoryEntity> repository;

	public CategoryController(IBaseRepository<CategoryEntity> repository, IMapper mapper)
	{
		this.repository = repository;
		this.mapper = mapper;
	}

	[HttpPost]
	public async Task<IActionResult> PostAsync(CategoryCreateRequest createRequest)
	{
		var entity = mapper.Map<CategoryEntity>(createRequest);
		await repository.CreateAsync(entity);

		var movieResponse = mapper.Map<CategoryModel>(entity);

		return CreatedAtRoute(nameof(GetAsync), new { id = movieResponse.Id }, movieResponse);
	}

	[HttpGet("{id}", Name = "GetCategoriesAsync")]
	public async Task<IActionResult> GetAsync(int id)
	{
		var entity = await repository.GetByIdAsync(id);
		var model = mapper.Map<CategoryEntity, CategoryModel>(entity);

		return Ok(model);
	}
}