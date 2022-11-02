using Api.CrossCutting.Mappings;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureAutoMapper
    {
        public static IMapper ConfigureMapper()
        {

            var config = new MapperConfiguration(c =>
            {
                c.AddProfile(new DtoToModelProfile());
                c.AddProfile(new EntityToDtoProfile());
                c.AddProfile(new ModelToEntityProfile());
            });

            IMapper mapper = config.CreateMapper();

            return mapper;
        }

    }
}
