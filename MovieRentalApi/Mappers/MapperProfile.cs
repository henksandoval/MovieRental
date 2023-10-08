using AutoMapper;
using MovieRentalApi.Data.Entities;
using MovieRentalApi.Models;
using MovieRentalApi.Requests;

namespace MovieRentalApi.Mappers;

public class MapperProfile : Profile
{
	public MapperProfile()
	{
		CreateMap<MovieEntity, MovieModel>().ReverseMap();
		CreateMap<MovieEntity, MovieCreateRequest>().ReverseMap();
		CreateMap<CategoryEntity, CategoryModel>().ReverseMap();
		CreateMap<CategoryEntity, CategoryCreateRequest>().ReverseMap();
	}
}