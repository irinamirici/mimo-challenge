namespace Mimo.Api.Messages
{
    public class Constants
    {
        public static class ErrorCodes
        {
            public const string CourseNotFound = "COURSE_NOT_FOUND";
            public const string ChapterNotFound = "CHAPTER_NOT_FOUND";
            public const string LessonNotFound = "LESSON_NOT_FOUND";
            public const string ClientUserNotFound = "CLIENT_USER_NOT_FOUND";
            public const string CourseAlreadyPublished = "COURSE_ALREADY_PUBLISHED";
            public const string CourseNotPublished = "COURSE_NOT_PUBLISHED";
            public const string BadFormat = "BAD_FORMAT";
            public const string Empty = "EMPTY_FIELD";
            public const string InvalidLength = "INVALID_LENGTH";
            public const string Duplicate = "DUPLICATE";
            public const string DateMustBeInThePast = "DATE_MUST_BE_IN_THE_PAST";
            public const string EndTimeMustBeGreaterThenStartTime= "END_TIME_MUST_BE_GREATER_THEN_START_TIME";
        }
    }
}