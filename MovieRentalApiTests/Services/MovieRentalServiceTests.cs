using FluentAssertions;
using MovieRentalApi.Mappers;
using MovieRentalApi.Models;
using MovieRentalApi.Services;
using MovieRentalApi.Utilities;

namespace MovieRentalApiTests.Services;

using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using NSubstitute;

public class MovieRentalServiceTests
{
    private readonly Fixture fixture = new Fixture();
    private readonly IBaseRepository<MovieEntity> repositoryMovie;
    private readonly IBaseRepository<RentalEntity> repositoryRental;
    private readonly MovieRentalService service;
    private readonly IMapper mapper;
    private readonly IClock clock;

    public MovieRentalServiceTests()
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<MapperProfile>();
        });

        mapper = new Mapper(configuration);
        repositoryMovie = Substitute.For<IBaseRepository<MovieEntity>>();
        repositoryRental = Substitute.For<IBaseRepository<RentalEntity>>();
        clock = Substitute.For<IClock>();
        service = new MovieRentalService(repositoryMovie, repositoryRental, clock, mapper);
    }

    [Fact(DisplayName = "MovieRentalService Cuando Solicitan Un Id de Pelicula Disponoble, debería responder la pelicula.")]
    public async Task MovieRentalService_WhenReceiveRequestIdMovieAndMovieIsAvailable_ShouldReturnTheMovie()
    {
        //Arrange (Preparar)
        var idMovie = 1;
        var entity = new MovieEntity{ IsAvailable = true };
        repositoryMovie.GetByIdAsync(idMovie).Returns(entity);
        
        //Act (Actuar)
        var movieResponse = await service.FindMovieAsync(idMovie);
        
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
        repositoryMovie.GetByIdAsync(idMovie).Returns(entity);
        
        //Act (Actuar)
        var movieResponse = await service.FindMovieAsync(idMovie);
        
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
        repositoryMovie.GetByIdAsync(idMovie).Returns(entity);

        //Act (Actuar)
        _ = await service.FindMovieAsync(idMovie);
        
        //Assert (Asegurar)
        await repositoryMovie.Received(1).UpdateAsync(Arg.Is<MovieEntity>(e => !e.IsAvailable));
    }

    [Fact(DisplayName = "MovieRentalService Cuando la Pelicula Esta Disponible, debe guardar la fecha de alquiler.")]
    public async Task MovieRentalService_WhenMovieIsAvailable_ShouldSaveRentalDateInDatabase()
    {
        //Arrange (Preparar)
        const int idMovie = 1;
        var entity = new MovieEntity{ IsAvailable = true };
        var expectedDate = new DateTime(1995, 05, 16);
        repositoryMovie.GetByIdAsync(idMovie).Returns(entity);
        
        clock.GetCurrentTime().Returns(expectedDate);

        //Act (Actuar)
        _ = await service.FindMovieAsync(idMovie);

        //Assert (Asegurar)
        await repositoryRental.Received(1).CreateAsync(Arg.Is<RentalEntity>(r => r.RentalDate == expectedDate));
    }
}