using Mimo.Api.UnitTests.Builders;
using Mimo.Api.Utils;
using Mimo.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mimo.Api.UnitTests.Utils
{
    public class UserAchievementsUpdaterTests
    {
        private IUserAchievementsUpdater updater;

        public UserAchievementsUpdaterTests()
        {
            updater = new UserAchievementsUpdater();
        }

        [Fact]
        public void LessonCompleted_IncrementsProgressOnlyForNotFinishedLessonAchievements()
        {
            //arrange
            var course = new CourseBuilder().Build();
            var typesToUpdate = new AchievementTypesToUpdate { LessonCompleted = true };
            var availableAchievements = new List<Achievement>
            {
                new AchievementBuilder().WithValues(3, AchievementType.CompleteLesson).Build(), //[0]
                new AchievementBuilder().WithValues(5, AchievementType.CompleteLesson).Build(), //[1]
                new AchievementBuilder().WithValues(2, AchievementType.CompleteChapter).Build(), //[2]
                new AchievementBuilder().WithValues(1, AchievementType.CompleteCourse).Build() //[3]
            };
            var user = new UserBuilder()
                .AddAchievement(availableAchievements[0], 1)
                .AddAchievement(availableAchievements[1], 1)
                .AddAchievement(availableAchievements[2], 0)
                .Build();
            var utcNow = DateTime.UtcNow;
            //act
            var achievementsUpdatedNow = updater.UpdateUserAchievements(user, course, typesToUpdate,
                availableAchievements, utcNow);
            //assert
            //modified achievements
            Assert.Equal(2, achievementsUpdatedNow.Count);
            Assert.Equal(2, achievementsUpdatedNow[0].Progress);
            Assert.False(achievementsUpdatedNow[0].IsCompleted);
            Assert.Equal(2, achievementsUpdatedNow[1].Progress);
            Assert.False(achievementsUpdatedNow[1].IsCompleted);

            //not modified achievements
            Assert.Equal(0, user.Achievements.ToList()[2].Progress);//chapter achievement remains unchanged
            Assert.Equal(3, user.Achievements.Count);
        }

        [Fact]
        public void LessonCompleted_DoesntIncrementProgressForFinishedLessonAchievements()
        {
            //arrange
            var course = new CourseBuilder().Build();
            var typesToUpdate = new AchievementTypesToUpdate { LessonCompleted = true };
            var availableAchievements = new List<Achievement>
            {
                new AchievementBuilder().WithValues(3, AchievementType.CompleteLesson).Build(), //[0]
                new AchievementBuilder().WithValues(5, AchievementType.CompleteLesson).Build(), //[1]
                new AchievementBuilder().WithValues(2, AchievementType.CompleteChapter).Build(), //[2]
                new AchievementBuilder().WithValues(1, AchievementType.CompleteCourse).Build() //[3]
            };
            var user = new UserBuilder()
                .AddAchievement(availableAchievements[0], 3)//isCompleted
                .AddAchievement(availableAchievements[1], 3)//3 out of 5
                .AddAchievement(availableAchievements[2], 1)//1 chapter out of 2
                .Build();
            var utcNow = DateTime.UtcNow;
            //act
            var achievementsUpdatedNow = updater.UpdateUserAchievements(user, course, typesToUpdate,
                availableAchievements, utcNow);

            //assert
            //modified achievements
            Assert.Single(achievementsUpdatedNow);
            Assert.Equal(4, achievementsUpdatedNow[0].Progress);
            Assert.False(achievementsUpdatedNow[0].IsCompleted);

            //not modified achievements
            Assert.Equal(3, user.Achievements.ToList()[0].Progress);//isCompleted lesson achievement remains unchanged
            Assert.Equal(1, user.Achievements.ToList()[2].Progress);//chapter achievement remains unchanged
            Assert.Equal(3, user.Achievements.Count);
        }

        [Fact]
        public void LessonCompleted_CompletesLessonAchievements()
        {
            //arrange
            var course = new CourseBuilder().Build();
            var typesToUpdate = new AchievementTypesToUpdate { LessonCompleted = true };
            var availableAchievements = new List<Achievement>
            {
                new AchievementBuilder().WithValues(3, AchievementType.CompleteLesson).Build(), //[0]
                new AchievementBuilder().WithValues(5, AchievementType.CompleteLesson).Build(), //[1]
                new AchievementBuilder().WithValues(2, AchievementType.CompleteChapter).Build(), //[2]
                new AchievementBuilder().WithValues(1, AchievementType.CompleteCourse).Build() //[3]
            };
            var user = new UserBuilder()
                .AddAchievement(availableAchievements[0], 3)//isCompleted
                .AddAchievement(availableAchievements[1], 4)//4 out of 5
                .AddAchievement(availableAchievements[2], 1)//1 chapter out of 2
                .Build();
            var utcNow = DateTime.UtcNow;
            //act
            var achievementsUpdatedNow = updater.UpdateUserAchievements(user, course, typesToUpdate,
                availableAchievements, utcNow);

            //assert
            //modified achievements
            Assert.Single(achievementsUpdatedNow);
            Assert.Equal(5, achievementsUpdatedNow[0].Progress);
            Assert.True(achievementsUpdatedNow[0].IsCompleted);
            Assert.Equal(utcNow, achievementsUpdatedNow[0].CompletedOn);

            //not modified achievements
            Assert.Equal(3, user.Achievements.ToList()[0].Progress);//isCompleted lesson achievement remains unchanged
            Assert.Equal(1, user.Achievements.ToList()[2].Progress);//chapter achievement remains unchanged
            Assert.Equal(3, user.Achievements.Count);
        }
        
        [Fact]
        public void LessonAndChapterCompleted_IncrementsProgressOnlyForNotFinishedChapterAchievements()
        {
            //arrange
            var course = new CourseBuilder().Build();
            var typesToUpdate = new AchievementTypesToUpdate { LessonCompleted = true, ChapterCompleted = true };
            var availableAchievements = new List<Achievement>
            {
                new AchievementBuilder().WithValues(3, AchievementType.CompleteLesson).Build(), //[0]
                new AchievementBuilder().WithValues(5, AchievementType.CompleteLesson).Build(), //[1]
                new AchievementBuilder().WithValues(3, AchievementType.CompleteChapter).Build(), //[2]
                new AchievementBuilder().WithValues(4, AchievementType.CompleteChapter).Build(), //[3]
                new AchievementBuilder().WithValues(1, AchievementType.CompleteCourse).Build() //[4]
            };
            var user = new UserBuilder()
                .AddAchievement(availableAchievements[0], 3)
                .AddAchievement(availableAchievements[1], 3)
                .AddAchievement(availableAchievements[2], 1)//1 chapter out of 3
                .AddAchievement(availableAchievements[3], 1)//1 chapter out of 4
                .Build();
            var utcNow = DateTime.UtcNow;
            //act
            var achievementsUpdatedNow = updater.UpdateUserAchievements(user, course, typesToUpdate,
                availableAchievements, utcNow);
            //assert
            //modified achievements
            Assert.Equal(3, achievementsUpdatedNow.Count); //1 lesson and 2 chapter achievements modified
            Assert.Equal(4, achievementsUpdatedNow[0].Progress);//lesson progress incremented
                                                                //chapters
            Assert.Equal(2, achievementsUpdatedNow[1].Progress);
            Assert.False(achievementsUpdatedNow[1].IsCompleted);
            Assert.Equal(2, achievementsUpdatedNow[2].Progress);
            Assert.False(achievementsUpdatedNow[2].IsCompleted);

            //no course achievements added
            Assert.Equal(4, user.Achievements.Count);
        }

        [Fact]
        public void LessonAndChapterCompleted_DoesntIncrementProgressForFinishedChapterAchievements()
        {
            //arrange
            var course = new CourseBuilder().Build();
            var typesToUpdate = new AchievementTypesToUpdate { LessonCompleted = true, ChapterCompleted = true };
            var availableAchievements = new List<Achievement>
            {
                new AchievementBuilder().WithValues(3, AchievementType.CompleteLesson).Build(), //[0]
                new AchievementBuilder().WithValues(5, AchievementType.CompleteLesson).Build(), //[1]
                new AchievementBuilder().WithValues(2, AchievementType.CompleteChapter).Build(), //[2]
                new AchievementBuilder().WithValues(5, AchievementType.CompleteChapter).Build(), //[3]
                new AchievementBuilder().WithValues(1, AchievementType.CompleteCourse).Build() //[4]
            };
            var user = new UserBuilder()
                .AddAchievement(availableAchievements[0], 3)//lesson achievement completed
                .AddAchievement(availableAchievements[1], 5)//lesson achievement completed
                .AddAchievement(availableAchievements[2], 2)//chapter achievement completed
                .AddAchievement(availableAchievements[3], 2)//2 chapters out of 5
                .Build();
            var utcNow = DateTime.UtcNow;
            //act
            var achievementsUpdatedNow = updater.UpdateUserAchievements(user, course, typesToUpdate,
                availableAchievements, utcNow);

            //assert
            //modified achievements
            Assert.Single(achievementsUpdatedNow);
            Assert.Equal(3, achievementsUpdatedNow[0].Progress);
            Assert.False(achievementsUpdatedNow[0].IsCompleted);

            //not modified achievements
            Assert.Equal(2, user.Achievements.ToList()[2].Progress);//isCompleted chapter achievement remains unchanged
            Assert.Equal(4, user.Achievements.Count);
        }

        [Fact]
        public void CourseCompleted_AddsCourseAchievement()
        {
            //arrange
            var course = new CourseBuilder().Build();
            var typesToUpdate = new AchievementTypesToUpdate
            {
                LessonCompleted = true,
                ChapterCompleted = true,
                CourseCompleted = true
            };
            var availableAchievements = new List<Achievement>
            {
                new AchievementBuilder().WithValues(3, AchievementType.CompleteLesson).Build(), //[0]
                new AchievementBuilder().WithValues(3, AchievementType.CompleteChapter).Build(), //[1]
                new AchievementBuilder().WithValues(1, AchievementType.CompleteCourse).Build() //[2]
            };
            var user = new UserBuilder()
                .AddAchievement(availableAchievements[0], 3)//isCompleted lesson achievement
                .AddAchievement(availableAchievements[1], 3)//isCompleted chapter acheievement
                .Build();
            var utcNow = DateTime.UtcNow;
            //act
            var achievementsUpdatedNow = updater.UpdateUserAchievements(user, course, typesToUpdate,
                availableAchievements, utcNow);

            //assert
            //modified achievements
            Assert.Single(achievementsUpdatedNow);
            Assert.Equal(1, achievementsUpdatedNow[0].Progress);
            Assert.True(achievementsUpdatedNow[0].IsCompleted);
            Assert.Equal(utcNow, achievementsUpdatedNow[0].CompletedOn);
            Assert.Equal(course, achievementsUpdatedNow[0].Course);
            Assert.Equal(3, user.Achievements.Count);//new achievement is added
            Assert.Equal(achievementsUpdatedNow[0], user.Achievements.Last());

            //not modified achievements
            Assert.Equal(3, user.Achievements.ToList()[0].Progress);//isCompleted lesson achievement remains unchanged
            Assert.Equal(3, user.Achievements.ToList()[1].Progress);//isCompleted chapter achievement remains unchanged

        }
    }
}