using Mimo.Persistence.Entities;
using System.Collections.Generic;

namespace Mimo.Api.UnitTests.Builders
{
    public class UserBuilder : BuilderBase<User>
    {
        public UserBuilder()
        {
            ObjectToBuild = new User
            {
                Id = 2,
                Username = "user",
                Achievements = new List<UserAchievement>(),
                CompletedLessons = new List<UserLesson>()
            };
        }

        public UserBuilder WithUsername(string username)
        {
            ObjectToBuild.Username = username;
            return this;
        }

        public UserBuilder AddAchievement(Achievement achievement, int progess)
        {
            ObjectToBuild.Achievements.Add(new UserAchievement
            {
                Achievement = achievement,
                Progress = progess,
                IsCompleted = achievement.Target == progess
            });
            return this;
        }


        public UserBuilder AddCompletedLesson(UserLesson lesson)
        {
            ObjectToBuild.CompletedLessons.Add(lesson);
            return this;
        }
    }
}
