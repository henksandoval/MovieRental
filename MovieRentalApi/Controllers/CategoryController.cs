using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Models;

namespace MovieRentalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IBaseRepository<CategoryEntity> _repository;
    private readonly IMapper _mapper;

    public CategoryController(IBaseRepository<CategoryEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CategoryCreateModel createModel)
    {
        var entity = _mapper.Map<CategoryEntity>(createModel);

        entity = await _repository.CreateAsync(entity);

        var movieResponse = _mapper.Map<CategoryModel>(entity);

        return CreatedAtRoute(nameof(GetAsync), new { id = movieResponse.Id }, movieResponse);
    }

    [HttpGet("{id}", Name = "GetAsync")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        var model = _mapper.Map<CategoryEntity, CategoryModel>(entity);

        return Ok(model);
    }
}