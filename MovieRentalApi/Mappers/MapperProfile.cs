using AutoMapper;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Models;

namespace MovieRentalApi.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<MovieEntity, MovieModel>().ReverseMap();
        }
    }
}
