using Microsoft.EntityFrameworkCore;
using Mimo.Api.Commands;
using Mimo.Api.IntegrationTests.Infrastructure;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Mimo.Api.IntegrationTests.Scenarios
{
    public class CompleteLessonScenarios : AbstractScenario
    {
        private List<int> usersToDelete;

        public CompleteLessonScenarios(TestServerFixture testServerFixture) : base(testServerFixture)
        {
            usersToDelete = new List<int>();
        }

        [Fact]
        public async Task UserGetsComplete5LessonsAchievementAsync()
        {
            //arrange
            var username = Guid.NewGuid().ToString();
            var user = AddUser(username);
            var client = GetHttpClientWithUserAuthorization(user);
            var completedLesson = new CompleteLessonCommand
            {
                StartTime = DateTime.UtcNow.AddHours(-2),
                EndTime = DateTime.UtcNow.AddHours(-1)
            };
            var beforeCompletion = DateTime.UtcNow;
            //act
            foreach (var lessonId in new List<int> { 2, 6, 4, 3, 2, 9 })
            {
                await client.PostAsJsonAsync($"/api/lessons/{lessonId}/actions/complete", completedLesson);
            };
            //assert
            var db = GetService<IMimoDbContext>();
            var updatedUser = db.Users.Include(x => x.Achievements).First(x => x.Username == username);
            var completedAchievement = updatedUser.Achievements
                .FirstOrDefault(x => x.Achievement.Type == AchievementType.CompleteLesson
                && x.Achievement.Target == 5);
            Assert.Equal(5, completedAchievement.Progress);
            Assert.True(completedAchievement.IsCompleted);
            Assert.True(completedAchievement.CompletedOn > beforeCompletion);
        }

        private User AddUser(string username)
        {
            var availableAchievements = dbContext.Achievements
                .Where(x => x.Type != AchievementType.CompleteCourse)
                .ToList();
            var userAch = new List<UserAchievement>();
            availableAchievements.ForEach(x => userAch.Add(new UserAchievement
            {
                Achievement = x,
                Progress = 0
            }));
            var user = new User
            {
                Username = username,
                Password = "pwd",
                Achievements = userAch
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            usersToDelete.Add(user.Id);
            return user;
        }

        private HttpClient GetHttpClientWithUserAuthorization(User user)
        {
            var encoded = base.Base64Encode($"{user.Username}:{user.Password}");
            var client = server.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {encoded}");

            return client;
        }
    }
}
