using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System.IO;
using System.Reflection;


namespace Mimo.Persistence.Migrations
{
    public class SeedHelper
    {
        private IConfiguration migrationConfiguration;

        public SeedHelper()
        {
            var basePath = Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.Location);
            migrationConfiguration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("migrationseed.json", optional: true, reloadOnChange: false)
                    .Build();
        }

        public string GetConnectionString()
        {
            var basePath = Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.Location);
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .Build();
            return configuration["ConnectionString"];
        }
        public IList<Course> GetCourses()
        {      
        return migrationConfiguration?.GetSection("courses").Get<List<Course>>();
        }

        public IList<Achievement> GetAchievements()
        {
           return migrationConfiguration?.GetSection("achievements").Get<List<Achievement>>();
        }

        public IList<User> GetUsers()
        {
            return migrationConfiguration?.GetSection("users").Get<List<User>>();
        }
    }
}
