using Newtonsoft.Json;

namespace Mimo.Api.Commands
{
    public class UpdateLessonCommand
    {
        [JsonIgnore]
        public int LessonId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }
    }
}
