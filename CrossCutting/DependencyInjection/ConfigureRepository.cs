using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(IServiceCollection serviceColletion)
        {
            serviceColletion.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            serviceColletion.AddScoped<IUserRepository, UserRepository>();

            if (Environment.GetEnvironmentVariable("DATABASE").ToUpper() == "SQLSERVER")
            {
                serviceColletion.AddDbContext<MyContext>(
                  options => options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION").ToLower())
                );

            }
        }
    }
}
