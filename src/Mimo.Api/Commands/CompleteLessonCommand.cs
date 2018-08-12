using Mimo.Persistence.Entities;
using Newtonsoft.Json;
using System;

namespace Mimo.Api.Commands
{
    public class CompleteLessonCommand
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [JsonIgnore]
        public int LessonId { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
    }
}
