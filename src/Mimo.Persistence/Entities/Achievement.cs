namespace Mimo.Persistence.Entities
{
    public class Achievement : Entity
    {
        public string Name { get; set; }

        public AchievementType Type { get; set; }

        public int Target { get; set; }

    }
}
