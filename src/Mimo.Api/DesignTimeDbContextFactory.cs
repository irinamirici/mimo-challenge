using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Mimo.Persistence.DbContexts;
using System.IO;

namespace Mimo.Api
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MimoDbContext>
    {
        public MimoDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<MimoDbContext>();
            var connectionString = configuration["ConnectionString"];
            builder.UseSqlite(connectionString,
                b => b.MigrationsAssembly("Mimo.Persistence"));
            return new MimoDbContext(builder.Options);
        }
    }
}
