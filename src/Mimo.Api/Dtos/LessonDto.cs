namespace Mimo.Api.Dtos
{
    public class LessonDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }
        
        public int Order { get; set; }

        public int ChapterId { get; set; }
    }
}
