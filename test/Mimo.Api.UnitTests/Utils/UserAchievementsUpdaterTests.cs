using System;
using System.Collections.Generic;
using System.Text;
using Mimo.Api.UnitTests.Builders;
using Mimo.Persistence.Entities;
using System.Collections.Generic;
using Xunit;
using Mimo.Api.Utils;

namespace Mimo.Api.UnitTests.Utils
{
  public  class UserAchievementsUpdaterTests
    {
        private IUserAchievementsUpdater updater;

        public UserAchievementsUpdaterTests()
        {
            updater = new UserAchievementsUpdater();
        }

        [Fact]
        public void UpdatesOnlyNotFinishedLessonAchievements()
        {

        }

        private User GetUser()
        {
            return new User
            {
                Achievements = new List<UserAchievement>
                 {
                     new UserAchievement
                     {
                          Progress = 0,
                           Achievement = new Achievement
                           {
                                Type = AchievementType.CompleteLesson,
                                 Target = 2
                           }
                     }
                 }
            };
        }

             
    }
}
