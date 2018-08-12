using Microsoft.Extensions.DependencyInjection;
using Mimo.Api.Commands.Handlers;
using Mimo.Api.Queries.Handlers;
using Mimo.Api.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mimo.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterGenericValidators(this IServiceCollection services/*, Type genericType, Type validatorType*/)
        {
            var assemblies = new List<Assembly> { Assembly.GetAssembly(typeof(CreateCourseCommandValidator)) };
            services.Scan(x =>
            {
                x.FromAssemblies(assemblies)
                    .AddClasses(cls => cls.AssignableTo(typeof(IResultValidator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime();
            });
        }

        public static void RegisterCommandHandlers(this IServiceCollection services)
        {
            var assemblies = new List<Assembly> { Assembly.GetAssembly(typeof(CompleteLessonCommandHandler)) };
            services.Scan(x =>
            {
                x.FromAssemblies(assemblies)
                    .AddClasses(cls => cls.AssignableTo(typeof(ICommandHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime();
            });
        }

        public static void RegisterQueryHandlers(this IServiceCollection services)
        {
            var assemblies = new List<Assembly> { Assembly.GetAssembly(typeof(CourseNameIsUniqueQueryHandler)) };
            services.Scan(x =>
            {
                x.FromAssemblies(assemblies)
                    .AddClasses(cls => cls.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime();
            });
        }
    }
}
