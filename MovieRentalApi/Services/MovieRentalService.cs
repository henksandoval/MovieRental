using AutoMapper;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Data.Repositories;
using MovieRentalApi.Models;

namespace MovieRentalApi.Services;

public class MovieRentalService
{
    private readonly IBaseRepository<MovieEntity> _repository;
    private readonly IMapper _mapper;

    public MovieRentalService(IBaseRepository<MovieEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MovieModel> FindMovieAsync(int idMovie)
    {
        var entity = await _repository.GetByIdAsync(idMovie);

        if (!entity.IsAvailable)
            return null;

        entity.IsAvailable = false;
        var movie = _mapper.Map<MovieModel>(entity);
        await _repository.UpdateAsync(entity);

        return movie;
    }
}