namespace Mimo.Api.Queries
{
    public class GetCourseQuery
    {
        public int CourseId { get; }
        public bool AllowPublishedCourse { get; }
        public bool IncludeChapters { get; }

        public GetCourseQuery(int courseId, bool allowPublishedCourse = true, bool includeChapters = false)
        {
            CourseId = courseId;
            AllowPublishedCourse = allowPublishedCourse;
            IncludeChapters = includeChapters;
        }
    }
}
