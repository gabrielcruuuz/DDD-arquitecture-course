using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        
        public MyContext CreateDbContext(string[] args)
        {
            //migrations
            string connectionString = "SERVER=localhost;DATABASE=DDD;UID=gabriel;PWD=56210160Casa!;";
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MyContext(optionsBuilder.Options);
        }
    }
}
