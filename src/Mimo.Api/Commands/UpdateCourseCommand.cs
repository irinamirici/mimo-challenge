using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimo.Api.Commands
{
    public class UpdateCourseCommand
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
