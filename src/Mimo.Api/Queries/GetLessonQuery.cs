namespace Mimo.Api.Queries
{
    public class GetLessonQuery
    {
        public int LessonId { get; }
        public bool AllowPublishedCourse { get; }

        public GetLessonQuery(int lessonId, bool allowPublishedCourse = true)
        {
            LessonId = lessonId;
            AllowPublishedCourse = allowPublishedCourse;
        }
    }
}
