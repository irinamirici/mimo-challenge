using Mimo.Persistence.Entities;
using System.Collections.Generic;

namespace Mimo.Api.UnitTests.Builders
{
    public class ChapterBuilder : BuilderBase<Chapter>
    {
        public ChapterBuilder()
        {
            ObjectToBuild = new Chapter
            {
                Id = 1,
                Lessons = new List<Lesson>()
            };
        }

        public ChapterBuilder WithChapterId(int id)
        {
            ObjectToBuild.Id = id;
            return this;
        }

        public ChapterBuilder WithLessonsWithIds(List<int> lessonIds)
        {
            lessonIds.ForEach(x =>
                 {
                     ObjectToBuild.Lessons.Add(new Lesson
                     {
                         Id = x
                     });
                 });
            return this;
        }
    }
}
