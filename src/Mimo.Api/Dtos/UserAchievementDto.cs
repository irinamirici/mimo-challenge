using Mimo.Persistence.Entities;
using Newtonsoft.Json;
using System;

namespace Mimo.Api.Dtos
{
    public class UserAchievementDto
    {
        public string AchievementIdentifier
        {
            get
            {
                return AchievementType == AchievementType.CompleteCourse
                    ? $"{AchievementId}-{Course?.Id}"
                    : AchievementId.ToString();
            }
        }

        public string Label
        {
            get
            {
                return AchievementType == AchievementType.CompleteCourse
                    ? string.Format(AchievementName, Course?.Name)
                    : AchievementName;
            }
        }

        public AchievementType AchievementType { get; set; }

        public string AchievementName { get; set; }

        public int Target { get; set; }

        [JsonIgnore]
        public int AchievementId { get; set; }

        [JsonIgnore]
        public Course Course { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedOn { get; set; }

        public int Progress { get; set; }

    }
}