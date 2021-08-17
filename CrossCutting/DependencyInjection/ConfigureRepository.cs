using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(IServiceCollection serviceColletion)
        {
            serviceColletion.AddDbContext<MyContext>(
                options => options.UseSqlServer("Server=.\\SQLEXPRESS;Database=DDD;user=gabrielCruz;password=56210160Casa")
            );

            serviceColletion.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            serviceColletion.AddScoped<IUserRepository, UserRepository>();

        }
    }
}
