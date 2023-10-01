using FluentAssertions;
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
    public async Task MovieRentalService_WhenReceiveRequestIdMovieAndMovieIsAvailable_ShouldReturnTheMovie()
    {
        //Arrange
        var idMovie = 1;
        var entity = new MovieEntity{ IsAvailable = true };
        _repository.GetByIdAsync(idMovie).Returns(entity);
        
        //Act
        var movieReponse = await _service.FindMovieAsync(idMovie);
        
        //Assert
        var expectedMovie = new MovieModel();
        movieReponse.Should().BeEquivalentTo(expectedMovie);
    }
    
    [Fact]
    public async Task MovieRentalService_WhenReceiveRequestIdMovieAndMovieIsNotAvailable_ShouldReturnNull()
    {
        //Arrange
        var idMovie = 1;
        var entity = new MovieEntity{ IsAvailable = false };
        _repository.GetByIdAsync(idMovie).Returns(entity);
        
        //Act
        var movieReponse = await _service.FindMovieAsync(idMovie);
        
        //Assert
        movieReponse.Should().BeNull();
    }
}