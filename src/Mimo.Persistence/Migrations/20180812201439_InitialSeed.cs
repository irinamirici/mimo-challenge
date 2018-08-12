using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Persistence.Migrations
{
    public partial class InitialSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            var seeder = new SeedHelper();
            var builder = new DbContextOptionsBuilder<MimoDbContext>()
                .UseSqlite(seeder.GetConnectionString());
            var courses = seeder.GetCourses();

            using (var context = new MimoDbContext(builder.Options))
            {
                if (courses != null)
                {
                    context.Courses.AddRange(courses);
                }
                var achievements = seeder.GetAchievements();
                if (achievements != null)
                {
                    context.Achievements.AddRange(achievements);
                }

                var users = seeder.GetUsers();
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        if (user.Role == UserRole.Client)
                        {
                            user.Achievements = new List<UserAchievement>();
                            foreach (var achievement in achievements.Where(x => x.Type != AchievementType.CompleteCourse))
                            {
                                user.Achievements.Add(new UserAchievement
                                {
                                    Achievement = achievement,
                                    IsCompleted = false,
                                    Progress = 0
                                });
                            }
                        }
                        context.Users.Add(user);
                    }
                }

                context.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var seeder = new SeedHelper();
            var builder = new DbContextOptionsBuilder<MimoDbContext>()
                .UseSqlite(seeder.GetConnectionString());

            using (var context = new MimoDbContext(builder.Options))
            {
                //todo delete inserted data
            }
        }
    }
}
