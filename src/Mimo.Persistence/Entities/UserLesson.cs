using System;
using System.Collections.Generic;
using System.Text;

namespace Mimo.Persistence.Entities
{
    public class UserLesson : Entity
    {
        public virtual User User { get; set; }

        public int LessonId { get; set; }

        public int ChapterId { get; set; }

        public int CourseId { get; set; }

        public DateTime LastStartedOn { get; set; }

        public DateTime LastFinishedOn { get; set; }

        public int NoOfCompletions { get; set; }
    }
}