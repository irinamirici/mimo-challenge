namespace Mimo.Api.Utils
{
    public class AchievementTypesToUpdate
    {
        //TODO we could use a Flags enum here instead
        public bool LessonCompleted { get; set; }
        public bool ChapterCompleted { get; set; }
        public bool CourseCompleted { get; set; }
    }
}
