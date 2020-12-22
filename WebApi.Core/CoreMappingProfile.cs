using AutoMapper;
using System;
using WebApi.Core.Enums;
using WebApi.Core.Models;
using WebApi.Core.Services.Articulos;
using WebApi.Core.Services.ClientApplications;
using WebApi.Core.Services.Estados;

namespace WebApi.Core
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            /*Example:
             *  CreateMap<DtoSource, DestinationClass>()
                   .ForMember(dest => dest.Property1, m => m.MapFrom(src => src.Property1))
                   .ForMember(dest => dest.Property2, m => m.MapFrom(src => src.Property2))
                   .ForMember(dest => dest.Property3, m => m.MapFrom(src => src.Property3))
             * 
            */
            CreateMap<ClientApplication, ClientApplicationDto>().ReverseMap()
                   .ForMember(dest => dest.Id, m => m.MapFrom(src => src.Id))
                   .ForMember(dest => dest.ClientName, m => m.MapFrom(src => src.ClientName))
                   .ForMember(dest => dest.IsActive, m => m.MapFrom(src => src.IsActive));

            CreateMap<ArticuloDto, Articulo>().ReverseMap()
                   .ForMember(dest => dest.Id, m => m.MapFrom(src => src.Id))
                   .ForMember(dest => dest.Nombre, m => m.MapFrom(src => src.Nombre))
                   .ForMember(dest => dest.Descripcion, m => m.MapFrom(src => src.Descripcion))
                   .ForMember(dest => dest.Precio1, m => m.MapFrom(src => src.Precio1))
                   .ForMember(dest => dest.Precio2, m => m.MapFrom(src => src.Precio2))
                   .ForMember(dest => dest.Codigo, m=> m.MapFrom(src=> src.Codigo))
                   .ForMember(dest => dest.EstadoArticulo , m=> m.MapFrom(src=> src.Estado))
                   .ForMember(dest => dest.Imagen, m=> m.MapFrom(src=> src.Imagen));

            CreateMap<EstadoArticuloDto, EstadoArticulo>().ReverseMap()
                   .ForMember(dest => dest.Id, m => m.MapFrom(src => src.Id))
                   .ForMember(dest => dest.Descripcion, m => m.MapFrom(src => src.Descripcion));
        }
    }




}
