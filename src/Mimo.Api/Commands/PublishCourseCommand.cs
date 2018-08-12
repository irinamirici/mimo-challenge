namespace Mimo.Api.Commands
{
    public class PublishCourseCommand
    {
        public int CourseId { get; }
        public PublishCourseCommand(int courseId)
        {
            CourseId = courseId;
        }
    }
}
