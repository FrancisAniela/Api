using AutoMapper;
using WebApi.ApiModels.Requests;
using WebApi.Core.Services.Articulos;

namespace WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /*Example:
             *  CreateMap<DtoSource, DestinationClass>()
                   .ForMember(dest => dest.Property1, m => m.MapFrom(src => src.Property1))
                   .ForMember(dest => dest.Property2, m => m.MapFrom(src => src.Property2))
                   .ForMember(dest => dest.Property3, m => m.MapFrom(src => src.Property3))
             * 
            */

            CreateMap<ArticuloRequest, ArticuloDto>()
                  .ForMember(dest => dest.Id, m => m.MapFrom(src => src.Id))
                  .ForMember(dest => dest.Nombre, m => m.MapFrom(src => src.Nombre))
                  .ForMember(dest => dest.Precio1, m => m.MapFrom(src => src.Precio))
                  .ForMember(dest => dest.Codigo, m => m.MapFrom(src => src.Codigo))
                  .ForMember(dest => dest.Descripcion, m => m.MapFrom(src => src.Descripcion));

        }
    }
}
