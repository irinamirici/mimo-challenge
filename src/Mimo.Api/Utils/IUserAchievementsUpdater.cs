using Mimo.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace Mimo.Api.Utils
{
    public interface IUserAchievementsUpdater
    {
        List<UserAchievement> UpdateUserAchievements(User user,
            Course course,
            AchievementTypesToUpdate achievementTypesToUpdate,
            ICollection<Achievement> systemAchievements,
            DateTime utcNow);
    }
}
