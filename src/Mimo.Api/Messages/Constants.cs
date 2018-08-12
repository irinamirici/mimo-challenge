namespace Mimo.Api.Messages
{
    public class Constants
    {
        public static class ErrorCodes
        {
            public const string CourseNotFound = "COURSE_NOT_FOUND";
            public const string ChapterNotFound = "CHAPTER_NOT_FOUND";
            public const string LessonNotFound = "LESSON_NOT_FOUND";
            public const string CourseAlreadyPublished = "COURSE_ALREADY_PUBLISHED";
            public const string CourseNotPublished = "COURSE_NOT_PUBLISHED";
            public const string BadFormat = "BAD_FORMAT";
            public const string Empty = "EMPTY_FIELD";
            public const string InvalidLength = "INVALID_LENGTH";
            public const string Duplicate = "DUPLICATE";
        }
    }
}