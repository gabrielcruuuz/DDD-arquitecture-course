using Api.CrossCutting.Mappings;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureAutoMapper
    {
        public static void ConfigureMapper(IServiceCollection serviceColletion)
        {

            var config = new MapperConfiguration(c =>
            {
                c.AddProfile(new DtoToModelProfile());
                c.AddProfile(new EntityToDtoProfile());
                c.AddProfile(new ModelToEntityProfile());
            });

            IMapper mapper = config.CreateMapper();

            serviceColletion.AddSingleton(mapper);

        }

    }
}
