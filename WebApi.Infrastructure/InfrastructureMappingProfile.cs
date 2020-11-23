using AutoMapper;

namespace WebApi.Infrastructure
{
    public class InfrastructureMappingProfile : Profile
    {
        public InfrastructureMappingProfile()
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
