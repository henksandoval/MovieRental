using MovieRentalApi.Mappers;
using MovieRentalApi.Models;
using MovieRentalApi.Services;

namespace MovieRentalApiTests.Services;

using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using NSubstitute;

public class MovieRentalServiceTests
{
    private readonly Fixture _fixture = new Fixture();
    private readonly IBaseRepository<MovieEntity> _repository;
    private readonly MovieRentalService _service;
    private readonly IMapper _mapper;

    public MovieRentalServiceTests()
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<MapperProfile>();
        });

        _mapper = new Mapper(configuration);
        _repository = Substitute.For<IBaseRepository<MovieEntity>>();
        _service = new MovieRentalService(_repository, _mapper);
    }

    [Fact]
    public async Task MovieRentalService_WhenReceivedIdMovie_ShouldReturnTheMovie()
    {
        //Arrange
        var expectedResponse = _fixture.Create<MovieModel>();
        var entity = _mapper.Map<MovieEntity>(expectedResponse);
        _repository.GetByIdAsync(expectedResponse.Id).Returns(entity);

        //Act
        var response = await _service.FindMovieAsync(expectedResponse.Id);

        //Assert
        Assert.Equivalent(expectedResponse, response);
    }
}