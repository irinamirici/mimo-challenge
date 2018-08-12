using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mimo.Api.FakeAuthentication;
using Mimo.Api.Infrastructure;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Mapping;
using Mimo.Api.Utils;
using Mimo.Persistence.DbContexts;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace Mimo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterDbContext(services);

            services.RegisterCommandHandlers();
            services.RegisterQueryHandlers();
            services.RegisterGenericValidators();
            services.AddScoped<IResponseHandler, ResponseHandler>();
            services.AddTransient<IUserAchievementsUpdater, UserAchievementsUpdater>();
            services.AddTransient<IAchievementTypesToUpdateCalculator, AchievementTypesToUpdateCalculator>();

            services.AddHttpContextAccessor();

            services.AddAuthentication("Basic")
                .AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>("Basic", null);

            services.AddAutoMapper(x => x.AddProfiles(new string[] { Assembly.GetAssembly(typeof(CommandToPersistenceProfile)).FullName }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MIMO API", Version = "v1" });
                c.OperationFilter<AddSwaggerAuthorizeHeaderFilter>();
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                ); 
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandlerMiddleware();
                app.UseAuthentication();
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MIMO API");
                });
            }
            else
            {
                app.UseExceptionHandlerMiddleware();
                app.UseHsts();
            }

            EnsureDbMigrated(app);

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        protected virtual void EnsureDbMigrated(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MimoDbContext>();
                context.Database.Migrate();
            }
        }
     
        protected virtual void RegisterDbContext(IServiceCollection services)
        {
            services.AddDbContext<MimoDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlite(Configuration["ConnectionString"],
                    b => b.MigrationsAssembly("Mimo.Persistence"));
            });
            services.AddScoped<IMimoDbContext, MimoDbContext>();
        }
    }
}
