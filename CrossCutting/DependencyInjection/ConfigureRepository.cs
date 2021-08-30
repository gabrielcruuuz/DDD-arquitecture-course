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

            var t = Environment.GetEnvironmentVariables();

            if (Environment.GetEnvironmentVariable("DATABASE").ToLower() == "SQLSERVER".ToLower())
            {
                serviceColletion.AddDbContext<MyContext>(
                  options => options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION").ToLower())
                );

            }
        }
    }
}
