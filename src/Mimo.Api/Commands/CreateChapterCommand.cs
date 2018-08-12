namespace Mimo.Api.Commands
{
    public class CreateChapterCommand
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public int CourseId { get; set; }
    }
}
