using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mimo.Persistence.DbContexts;
using System;

namespace Mimo.Api.IntegrationTests.Infrastructure
{
    public class IntegrationTestsStartup : Startup
    {
        public IntegrationTestsStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void RegisterDbContext(IServiceCollection services)
        {
            services.AddDbContext<MimoDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlite("Data Source=mimotest.db",
                    b => b.MigrationsAssembly("Mimo.Persistence"));
            });
            services.AddTransient<IMimoDbContext, MimoDbContext>();
        }

        protected override void EnsureDbMigrated(IApplicationBuilder app)
        {
            try
            {
                base.EnsureDbMigrated(app);
            }
            catch (Exception)
            {

            }
        }
    }
}
