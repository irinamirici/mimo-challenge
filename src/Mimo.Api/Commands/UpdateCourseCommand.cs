using Newtonsoft.Json;

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
