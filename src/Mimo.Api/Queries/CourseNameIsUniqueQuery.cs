namespace Mimo.Api.Queries
{
    public class CourseNameIsUniqueQuery
    {
        public string Name { get; }
        public int CourseId { get; }

        public CourseNameIsUniqueQuery(int courseId, string name)
        {
            CourseId = courseId;
            Name = name;
        }
    }
}
