using AutoMapper;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Models;
using MovieRentalApi.Utilities;

namespace MovieRentalApi.Services;

public class MovieRentalService
{
    private readonly IBaseRepository<MovieEntity> repositoryMovie;
    private readonly IBaseRepository<RentalEntity> repositoryRental;
    private readonly IClock clock;
    private readonly IMapper mapper;

    public MovieRentalService(IBaseRepository<MovieEntity> repositoryMovie,
        IBaseRepository<RentalEntity> repositoryRental,
        IClock clock,
        IMapper mapper)
    {
        this.repositoryMovie = repositoryMovie;
        this.repositoryRental = repositoryRental;
        this.clock = clock;
        this.mapper = mapper;
    }

    public async Task<MovieModel> FindMovieAsync(int idMovie)
    {
        var movieEntity = await repositoryMovie.GetByIdAsync(idMovie);

        if (!movieEntity.IsAvailable)
            return null;

        movieEntity.IsAvailable = false;
        var movie = mapper.Map<MovieModel>(movieEntity);
        var rentalEntity = new RentalEntity { RentalDate = clock.GetCurrentTime() };

        await repositoryMovie.UpdateAsync(movieEntity);
        await repositoryRental.CreateAsync(rentalEntity);

        return movie;
    }
}