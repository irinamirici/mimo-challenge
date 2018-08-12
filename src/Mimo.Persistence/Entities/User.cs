using System.Collections.Generic;

namespace Mimo.Persistence.Entities
{
    public class User: Entity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        public virtual ICollection<UserAchievement> Achievements { get; set; }
        public virtual ICollection<UserLesson> CompletedLessons { get; set; }
        
        public virtual void AddCompletedLesson(UserLesson lesson)
        {
            if (CompletedLessons == null)
            {
                CompletedLessons = new List<UserLesson>();
            }
       
            CompletedLessons.Add(lesson);
        }
    }
}
