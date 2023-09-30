using MovieRentalApi.Models;

namespace MovieRentalApi.Services;

public class MovieRentalService
{
    public async Task<MovieModel> FindMovieAsync(int idMovie)
    {
        var movie = new MovieModel { Id = idMovie };
        return movie;
    }
}