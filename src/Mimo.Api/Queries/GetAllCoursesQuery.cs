namespace Mimo.Api.Queries
{
    public class GetAllCoursesQuery
    {
        public bool GetOnlyPublishedCourses { get; }

        public GetAllCoursesQuery(bool getOnlyPublishedCourses = false)
        {
            GetOnlyPublishedCourses = getOnlyPublishedCourses;
        }
    }
}
