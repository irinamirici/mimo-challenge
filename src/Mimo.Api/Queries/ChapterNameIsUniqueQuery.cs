namespace Mimo.Api.Queries
{
    public class ChapterNameIsUniqueQuery
    {
        public int CourseId { get; }
        public int ChapterId { get; }
        public string ChapterName { get; }

        public ChapterNameIsUniqueQuery(int courseId, int chapterId, string chapterName)
        {
            CourseId = courseId;
            ChapterId = chapterId;
            ChapterName = chapterName;
        }
    }
}
