namespace Mimo.Api.Queries
{
    public class GetChapterQuery
    {
        public int ChapterId { get; }
        public bool AllowPublishedCourse { get; }
        public bool IncludeLessons { get; }

        public GetChapterQuery(int chapterId, bool allowPublishedCourse = true, bool includeLessons = false)
        {
            ChapterId = chapterId;
            AllowPublishedCourse = allowPublishedCourse;
            IncludeLessons = includeLessons;
        }
    }
}
