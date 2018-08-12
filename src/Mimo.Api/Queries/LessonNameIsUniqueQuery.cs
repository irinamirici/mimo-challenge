namespace Mimo.Api.Queries
{
    public class LessonNameIsUniqueQuery
    {
        public int LessonId { get; }
        public int ChapterId { get; }
        public string LessonName { get; }

        public LessonNameIsUniqueQuery(int lessonId, int chapterId, string lessonName)
        {
            LessonId = lessonId;
            ChapterId = chapterId;
            LessonName = lessonName;
        }
    }
}
