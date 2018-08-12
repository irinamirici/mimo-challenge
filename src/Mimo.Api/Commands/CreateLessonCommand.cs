namespace Mimo.Api.Commands
{
    public class CreateLessonCommand
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }

        public int ChapterId { get; set; }
    }
}
