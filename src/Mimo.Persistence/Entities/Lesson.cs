using System;
using System.Collections.Generic;
using System.Text;

namespace Mimo.Persistence.Entities
{
   public class Lesson:Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }

        public virtual Chapter Chapter { get; set; }
    }
}
