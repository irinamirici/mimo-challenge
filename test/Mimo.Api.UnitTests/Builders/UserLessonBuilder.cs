using Mimo.Persistence.Entities;
using System;

namespace Mimo.Api.UnitTests.Builders
{
    public class UserLessonBuilder : BuilderBase<UserLesson>
    {
        public UserLessonBuilder()
        {
            ObjectToBuild = new UserLesson
            {
                Id = 1,
                LessonId = 1,
                ChapterId = 1,
                CourseId = 1,
                LastStartedOn = DateTime.UtcNow.AddHours(-2),
                LastFinishedOn = DateTime.UtcNow,
                NoOfCompletions = 1
            };
        }

        public UserLessonBuilder WithLessonData(int lessonId, int chapterId, int courseId)
        {
            ObjectToBuild.LessonId = lessonId;
            ObjectToBuild.ChapterId = chapterId;
            ObjectToBuild.CourseId = courseId;
            return this;
        }
    }
}
