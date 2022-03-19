using Api.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using Xunit;

namespace Api.Test
{
    public abstract class BaseTestData
    {
        public BaseTestData()
        {}
    }

    public class DbTest : IDisposable
    {
        private string dataBaseName = $"dbApiTest_{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
        public ServiceProvider serviceProvider { get; private set; }

        public DbTest()
        {
            var serviceColletion = new ServiceCollection();
            serviceColletion.AddDbContext<MyContext>(o =>
               o.UseSqlServer($"Server=.\\SQLEXPRESS;Database={dataBaseName};user=gabrielCruz;password=56210160Casa"),
                    ServiceLifetime.Transient
            );

            serviceProvider = serviceColletion.BuildServiceProvider();
            using (var context = serviceProvider.GetService<MyContext>()) 
            {
                context.Database.EnsureCreated();    
            }
        }

        public void Dispose()
        {
            using (var context = serviceProvider.GetService<MyContext>())
            {
                context.Database.EnsureDeleted();
            }
        }

    }
}
