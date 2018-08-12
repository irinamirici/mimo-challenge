using System;

namespace Mimo.Persistence.Entities
{
    public class UserAchievement:Entity
    {
        public virtual User User { get; set; }

        public virtual Achievement Achievement { get; set; }

        public virtual Course Course { get; set; }

        public int Progress { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedOn { get; set; }
    }
}
