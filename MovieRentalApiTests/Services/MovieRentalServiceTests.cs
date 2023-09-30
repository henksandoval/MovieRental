using MovieRentalApi.Models;
using MovieRentalApi.Services;

namespace MovieRentalApiTests.Services;

using System.Threading.Tasks;

public class MovieRentalServiceTests
{
    [Fact]
    public async Task MovieRentalService_WhenReceivedIdMovie_ShouldReturnTheMovie()
    {
        //Arrange
        int idMovie = 1;
        var service = new MovieRentalService();

        //Act
        MovieModel response = await service.FindMovieAsync(idMovie);

        //Assert
        var expectedResponse = new MovieModel { Id = idMovie };
        Assert.Equivalent(expectedResponse, response);
    }

    [Fact]
    public async Task MovieRentalService_WhenReceivedIdMovie_ShouldReturnTheMovie2()
    {
        //Arrange
        int idMovie = 2;
        var service = new MovieRentalService();

        //Act
        MovieModel response = await service.FindMovieAsync(idMovie);

        //Assert
        var expectedResponse = new MovieModel { Id = idMovie };
        Assert.Equivalent(expectedResponse, response);
    }
}