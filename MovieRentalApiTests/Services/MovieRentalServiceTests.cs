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

    [Fact(DisplayName = "MovieRentalService Cuando Solicitan Un Id de Pelicula Disponoble, debería responder la pelicula.")]
    public async Task MovieRentalService_WhenReceiveRequestIdMovieAndMovieIsAvailable_ShouldReturnTheMovie()
    {
        //Arrange (Preparar)
        var idMovie = 1;
        var entity = new MovieEntity{ IsAvailable = true };
        _repository.GetByIdAsync(idMovie).Returns(entity);
        
        //Act (Actuar)
        var movieResponse = await _service.FindMovieAsync(idMovie);
        
        //Assert (Asegurar)
        var expectedMovie = new MovieModel();
        movieResponse.Should().BeEquivalentTo(expectedMovie);
    }

    [Fact(DisplayName = "MovieRentalService Cuando Solicitan Un Id de Pelicula No Disponoble, debería responder nulo.")]
    public async Task MovieRentalService_WhenReceiveRequestIdMovieAndMovieIsNotAvailable_ShouldReturnNull()
    {
        //Arrange (Preparar)
        var idMovie = 1;
        var entity = new MovieEntity{ IsAvailable = false };
        _repository.GetByIdAsync(idMovie).Returns(entity);
        
        //Act (Actuar)
        var movieResponse = await _service.FindMovieAsync(idMovie);
        
        //Assert (Asegurar)
        movieResponse.Should().BeNull();
    }

    [Fact(DisplayName =
        "MovieRentalService Cuando La Pelicula Esta Disponible, Debe Registrar en Base de datos que esta alquilada.")]
    public async Task MovieRentalService_WhenMovieIsAvailable_ShouldSaveInDatabase()
    {
        //Arrange (Preparar)
        const int idMovie = 1;
        var entity = new MovieEntity{ IsAvailable = true };
        _repository.GetByIdAsync(idMovie).Returns(entity);

        //Act (Actuar)
        _ = await _service.FindMovieAsync(idMovie);
        
        //Assert (Asegurar)
        await _repository.Received(1).UpdateAsync(Arg.Is<MovieEntity>(e => !e.IsAvailable));
    }
}