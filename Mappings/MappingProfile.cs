using AutoMapper;
using Libros.Models;
using Libros.DTOs;

namespace Libros.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Libro, LibroDTO>().ReverseMap();
            CreateMap<Libro, CreateLibroDTO>().ReverseMap();
            CreateMap<Libro, UpdateLibroDTO>().ReverseMap();
        }
    }
}
