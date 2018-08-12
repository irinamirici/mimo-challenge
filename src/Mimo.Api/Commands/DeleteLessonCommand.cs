namespace Mimo.Api.Commands
{
    public class DeleteLessonCommand
    {
        public int LessonId { get; }
        public DeleteLessonCommand(int lessonId)
        {
            LessonId = lessonId;
        }
    }
}
