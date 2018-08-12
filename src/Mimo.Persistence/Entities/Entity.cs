using System;
using System.Collections.Generic;
using System.Text;

namespace Mimo.Persistence.Entities
{
    public class Entity
    {
        public int Id { get; set; }

        public bool IsNew()
        {
            return Id <= 0;
        }
    }
}