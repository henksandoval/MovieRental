using Microsoft.AspNetCore.Mvc;
using MovieRentalApi.Mappers;

namespace MovieRentalApiTests.Controllers;

using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using MovieRentalApi.Controllers;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Models;
using NSubstitute;

public class CategoryControllerTests
{
    private readonly CategoryController _controller;
    private readonly IBaseRepository<CategoryEntity> _repository;
    private readonly Fixture _fixture = new Fixture();
    private readonly IMapper _mapper;

    public CategoryControllerTests()
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<MapperProfile>();
        });

        _mapper = new Mapper(configuration);
        _repository = Substitute.For<IBaseRepository<CategoryEntity>>();
        _controller = new CategoryController(_repository, _mapper);
    }

    [Fact]
    public async Task CategoryController_WhenReceiveACategoryCreateModel_ShouldReturnOk()
    {
        //Arrange
        var createModel = _fixture.Create<CategoryCreateModel>();
        var entity = _mapper.Map<CategoryEntity>(createModel);
        entity.Id = 1;
        _repository.CreateAsync(Arg.Any<CategoryEntity>()).Returns(entity);

        //Act
        var response = await _controller.PostAsync(createModel);

        //Assert
        var result = response as CreatedAtRouteResult;
        var modelResult = result.Value as CategoryModel;

        var expected = _mapper.Map<CategoryModel>(entity);
        var expectedResponse = new CreatedAtRouteResult(nameof(CategoryController.GetAsync), new { id = entity.Id }, expected);

        Assert.Equivalent(expectedResponse, result);
        Assert.Equivalent(expected, modelResult);
    }

    [Fact]
    public async Task CategoryController_WhenReceivedAnIdentifier_ShouldReturnAnCategoryModel()
    { 
        //Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var entity = _fixture.Create<CategoryEntity>();

        var modelExpected = _mapper.Map<CategoryModel>(entity);
        _repository.GetByIdAsync(Arg.Any<int>()).Returns(entity);

        //Act
        var response = await _controller.GetAsync(entity.Id);

        //Assert
        var result = response as OkObjectResult;
        var modelResponse = result.Value as CategoryModel;
        Assert.Equivalent(modelExpected, modelResponse);
    }
}