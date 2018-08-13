using Mimo.Persistence.Entities;

namespace Mimo.Api.UnitTests.Builders
{
    public class AchievementBuilder : BuilderBase<Achievement>
    {
        public AchievementBuilder()
        {
            ObjectToBuild = new Achievement
            {
                Name = "ach 1",
                Type = AchievementType.CompleteLesson,
                Target = 3
            };
        }

        public AchievementBuilder WithValues(int target, AchievementType type)
        {
            ObjectToBuild.Target = target;
            ObjectToBuild.Type = type;
            return this;
        }
    }
}
