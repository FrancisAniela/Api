using AutoMapper;

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
        }
    }
}
