using AutoMapper;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Models;

namespace MovieRentalApi.Mappers;

public class MapperProfile : Profile
{
	public MapperProfile()
	{
		CreateMap<MovieEntity, MovieModel>().ReverseMap();
		CreateMap<MovieEntity, MovieCreateModel>().ReverseMap();
		CreateMap<CategoryEntity, CategoryModel>().ReverseMap();
		CreateMap<CategoryEntity, CategoryCreateModel>().ReverseMap();
	}
}