using AutoMapper;
using WebApplicationGeometryData.Models.DTOs;
using WebApplicationGeometryData.Models;

namespace WebApplicationGeometryData.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GeometryDto, GeometryData>().ReverseMap();
            CreateMap<SpatialReferenceDto, SpatialReference>().ReverseMap();
            CreateMap<SymbolDto, Symbol>().ReverseMap();
            CreateMap<OutlineDto, Outline>().ReverseMap();
        }
    }
}
