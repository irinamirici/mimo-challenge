using System;
using System.Collections.Generic;
using System.Text;

namespace Mimo.Persistence.Entities
{
    public class Chapter:Entity
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public string Description { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
