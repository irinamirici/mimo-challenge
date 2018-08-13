using Mimo.Persistence.Entities;
using System.Collections.Generic;

namespace Mimo.Api.UnitTests.Builders
{
    public class CourseBuilder : BuilderBase<Course>
    {
        public CourseBuilder()
        {
            ObjectToBuild = new Course
            {
                Id = 1,
                IsPublished = true,
                Chapters = new List<Chapter>()
            };
        }

        public CourseBuilder WithCourseId(int id)
        {
            ObjectToBuild.Id = id;
            return this;
        }

        public CourseBuilder AddChapter(Chapter chapter)
        {
            ObjectToBuild.Chapters.Add(chapter);
            return this;
        }

        public CourseBuilder Unpublished()
        {
            ObjectToBuild.IsPublished = false;
            return this;
        }
    }
}
