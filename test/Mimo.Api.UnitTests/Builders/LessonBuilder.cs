using System;
using Mimo.Persistence.Entities;

namespace Mimo.Api.UnitTests.Builders
{
    public class LessonBuilder : BuilderBase<Lesson>
    {
        public LessonBuilder()
        {
            ObjectToBuild = new Lesson
            {
                Id = 4,
                Chapter = new Chapter()
            };
        }

        public LessonBuilder WithId(int lessonId)
        {
            ObjectToBuild.Id = lessonId;
            return this;
        }

        public LessonBuilder WithUnpublishedCourse()
        {
            ObjectToBuild.Chapter = new ChapterBuilder()
                .WithUnpblishedCourse()
                .Build();
            return this;
        }

        public LessonBuilder WithPublishedCourse(int courseId)
        {
            ObjectToBuild.Chapter = new ChapterBuilder()
               .WithCourse( new CourseBuilder().WithCourseId(courseId).Build())
               .Build();
            return this;
        }
    }
}
