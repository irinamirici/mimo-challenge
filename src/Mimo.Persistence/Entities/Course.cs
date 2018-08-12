using System;
using System.Collections.Generic;
using System.Text;

namespace Mimo.Persistence.Entities
{
    public class Course:Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPublished { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }

    }
}
