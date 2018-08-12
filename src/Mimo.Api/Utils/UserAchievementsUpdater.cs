using Mimo.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Api.Utils
{
    public class UserAchievementsUpdater : IUserAchievementsUpdater
    {
        public List<UserAchievement> UpdateUserAchievements(User user,
            Course course,
            AchievementTypesToUpdate achievementTypesToUpdate,
            ICollection<Achievement> systemAchievements,
            DateTime utcNow)
        {
            var userAchievements = user.Achievements;
            var achievementsUpdatedNow = new List<UserAchievement>();

            foreach (var notCompletedAchievement in userAchievements
                .Where(x => !x.IsCompleted && x.Achievement.Type != AchievementType.CompleteCourse))
            {
                if (LessonOrChapterTypeAchievementsMustBeUpdates(achievementTypesToUpdate, notCompletedAchievement.Achievement.Type))
                {
                    UpdateAchievement(notCompletedAchievement, achievementsUpdatedNow, utcNow);
                }
            }

            if (achievementTypesToUpdate.CourseCompleted)
            {
                var completedCourseAchievement = new UserAchievement
                {
                    Achievement = systemAchievements.First(x => x.Type == AchievementType.CompleteCourse),
                    IsCompleted = true,
                    CompletedOn = utcNow,
                    Progress = 1,
                    Course = course
                };
                user.Achievements.Add(completedCourseAchievement);
                achievementsUpdatedNow.Add(completedCourseAchievement);
            }

            return achievementsUpdatedNow;
        }

        private bool LessonOrChapterTypeAchievementsMustBeUpdates(AchievementTypesToUpdate achievementTypesToUpdate, AchievementType userAchievementType)
        {
            return (achievementTypesToUpdate.LessonCompleted && userAchievementType == AchievementType.CompleteLesson)
                || (achievementTypesToUpdate.ChapterCompleted && userAchievementType == AchievementType.CompleteChapter);
        }

        private void UpdateAchievement(UserAchievement userAchievement,
            List<UserAchievement> achievementsUpdatedNow,
            DateTime utcNow)
        {
            userAchievement.Progress++;
            if (userAchievement.Progress == userAchievement.Achievement.Target)
            {
                userAchievement.IsCompleted = true;
                userAchievement.CompletedOn = utcNow;
            }
            achievementsUpdatedNow.Add(userAchievement);
        }
    }
}
