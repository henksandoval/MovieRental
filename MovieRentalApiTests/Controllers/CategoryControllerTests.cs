﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Controllers;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Mappers;
using MovieRentalApi.Models;

namespace MovieRentalApiTests.Controllers;

public class CategoryControllerTests
{
	private readonly CategoryController controller;
	private readonly Fixture fixture = new();
	private readonly IMapper mapper;
	private readonly IBaseRepository<CategoryEntity?> repository;

	public CategoryControllerTests()
	{
		var configuration = new MapperConfiguration(config => { config.AddProfile<MapperProfile>(); });

		mapper = new Mapper(configuration);
		repository = Substitute.For<IBaseRepository<CategoryEntity>>();
		controller = new CategoryController(repository, mapper);
	}

	[Fact]
	public async Task CategoryController_WhenReceiveACategoryCreateModel_ShouldReturnOk()
	{
		//Arrange
		var createModel = fixture.Create<CategoryCreateModel>();
		var entity = mapper.Map<CategoryEntity>(createModel);
		entity.Id = 1;
		repository.CreateAsync(Arg.Any<CategoryEntity>()).Returns(entity);

		//Act
		var response = await controller.PostAsync(createModel);

		//Assert
		var result = response as CreatedAtRouteResult;
		var modelResult = result?.Value as CategoryModel;

		var expectedModel = mapper.Map<CategoryModel>(entity);
		var expectedResult =
			new CreatedAtRouteResult(nameof(CategoryController.GetAsync), new { id = entity.Id }, expectedModel);

		result.Should().BeEquivalentTo(expectedResult);
		modelResult.Should().BeEquivalentTo(expectedModel);
	}

	[Fact]
	public async Task CategoryController_WhenReceivedAnIdentifier_ShouldReturnAnCategoryModel()
	{
		//Arrange
		fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
			.ForEach(b => fixture.Behaviors.Remove(b));
		fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

		var entity = fixture.Create<CategoryEntity>();

		var modelExpected = mapper.Map<CategoryModel>(entity);
		repository.GetByIdAsync(Arg.Any<int>()).Returns(entity);

		//Act
		var response = await controller.GetAsync(entity.Id);

		//Assert
		var result = response as OkObjectResult;
		var modelResponse = result?.Value as CategoryModel;

		modelResponse.Should().BeEquivalentTo(modelExpected);
	}
}