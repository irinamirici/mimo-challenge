using Mimo.Persistence.Entities;
using System.Collections.Generic;

namespace Mimo.Api.Utils
{
    public interface IAchievementTypesToUpdateCalculator
    {
        AchievementTypesToUpdate CalculateAchievementTypesToUpdate(ICollection<UserLesson> userCompletedLessons, Course currentCourse, int chapterId);
    }
}
