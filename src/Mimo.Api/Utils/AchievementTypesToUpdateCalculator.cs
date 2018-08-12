using Mimo.Persistence.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Api.Utils
{
    public class AchievementTypesToUpdateCalculator : IAchievementTypesToUpdateCalculator
    {
        public AchievementTypesToUpdate CalculateAchievementTypesToUpdate(ICollection<UserLesson> userCompletedLessons, Course currentCourse, int chapterId)
        {
            var achievementTypesToUpdate = new AchievementTypesToUpdate
            {
                LessonCompleted = true
            };
            var currentChapter = currentCourse.Chapters.First(x => x.Id == chapterId);
            if (UserHasCompletedAllLessonsForCurrentChapter(userCompletedLessons, currentChapter))
            {
                achievementTypesToUpdate.ChapterCompleted = true;
            }
            if (UserHasCompletedAllLessonsForCurrentCourse(userCompletedLessons, currentCourse))
            {
                achievementTypesToUpdate.CourseCompleted = true;
                achievementTypesToUpdate.CourseId = currentCourse.Id;
            }
            return achievementTypesToUpdate;
        }

        private bool UserHasCompletedAllLessonsForCurrentChapter(ICollection<UserLesson> userCompletedLessons, Chapter currentChapter)
        {
            return userCompletedLessons.Count(x => x.ChapterId == currentChapter.Id) == currentChapter.Lessons.Count();
        }

        private bool UserHasCompletedAllLessonsForCurrentCourse(ICollection<UserLesson> userCompletedLessons, Course currentCourse)
        {
            return userCompletedLessons.Count(x => x.CourseId == currentCourse.Id) == currentCourse.Chapters.SelectMany(x => x.Lessons).Count();
        }
    }
}
