using Newtonsoft.Json;

namespace Mimo.Api.Commands
{
    public class UpdateChapterCommand
    {
        [JsonIgnore]
        public int ChapterId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
    }
}
