using Mimo.Api.UnitTests.Builders;
using Mimo.Persistence.Entities;
using System.Collections.Generic;
using Xunit;

namespace Mimo.Api.Utils.UnitTests
{
    public class AchievementTypesToUpdateCalculatorTests
    {
        private IAchievementTypesToUpdateCalculator calculator;

        public AchievementTypesToUpdateCalculatorTests()
        {
            calculator = new AchievementTypesToUpdateCalculator();
        }

        [Fact]
        public void WhenThereAreLessonsLeftInCourseAndChapter_ReturnsOnlyLessonAchievement()
        {
            var userCompletedLessons = new List<UserLesson> {
                //lesson from current course
                new UserLessonBuilder()
                    .WithLessonData(lessonId: 30, chapterId: 3, courseId: 2)
                    .Build(),
                //data from other course
                new UserLessonBuilder()
                    .WithLessonData(lessonId: 77, chapterId: 7, courseId: 1)
                    .Build()
            };

            var course = GetCourseWith2Chapters();

            //act
            var achievementTypesToUpdate = calculator.CalculateAchievementTypesToUpdate(
                userCompletedLessons,
                course,
                3);

            //assert
            Assert.True(achievementTypesToUpdate.LessonCompleted);
            Assert.False(achievementTypesToUpdate.ChapterCompleted);
            Assert.False(achievementTypesToUpdate.CourseCompleted);
        }

        [Fact]
        public void WhenAllLessonsInChapterAreFinished_ReturnsLessonAndChapterAchievement()
        {
            var userCompletedLessons = new List<UserLesson> {
                //lesson from other chapter
                new UserLessonBuilder().WithLessonData(lessonId: 41, chapterId: 4, courseId: 2).Build(),
                //lesson from current course
                new UserLessonBuilder().WithLessonData(lessonId: 31, chapterId: 3, courseId: 2).Build(),
                new UserLessonBuilder().WithLessonData(lessonId: 30, chapterId: 3, courseId: 2).Build(),
                new UserLessonBuilder().WithLessonData(lessonId: 32, chapterId: 3, courseId: 2).Build(),
                //data from other course
                new UserLessonBuilder().WithLessonData(lessonId: 77, chapterId: 7, courseId: 1).Build()
            };

            var course = GetCourseWith2Chapters();

            //act
            var achievementTypesToUpdate = calculator.CalculateAchievementTypesToUpdate(
                userCompletedLessons,
                course,
                3);

            //assert
            Assert.True(achievementTypesToUpdate.LessonCompleted);
            Assert.True(achievementTypesToUpdate.ChapterCompleted);
            Assert.False(achievementTypesToUpdate.CourseCompleted);
        }

        [Fact]
        public void WhenAllLessonsInCourseAreFinished_ReturnsAllAchievementTypes()
        {
            var userCompletedLessons = new List<UserLesson> {
                //lesson from current course - chapter 3
                new UserLessonBuilder().WithLessonData(lessonId: 31, chapterId: 3, courseId: 2).Build(),
                new UserLessonBuilder().WithLessonData(lessonId: 30, chapterId: 3, courseId: 2).Build(),
                new UserLessonBuilder().WithLessonData(lessonId: 32, chapterId: 3, courseId: 2).Build(),
                //lesson from current course - chapter 4
                new UserLessonBuilder().WithLessonData(lessonId: 40, chapterId: 4, courseId: 2).Build(),
                new UserLessonBuilder().WithLessonData(lessonId: 41, chapterId: 4, courseId: 2).Build(),
                //data from other course
                new UserLessonBuilder().WithLessonData(lessonId: 77, chapterId: 7, courseId: 1).Build()
            };

            var course = GetCourseWith2Chapters();

            //act
            var achievementTypesToUpdate = calculator.CalculateAchievementTypesToUpdate(
                userCompletedLessons,
                course,
                3);

            //assert
            Assert.True(achievementTypesToUpdate.LessonCompleted);
            Assert.True(achievementTypesToUpdate.ChapterCompleted);
            Assert.True(achievementTypesToUpdate.CourseCompleted);
        }

        private Course GetCourseWith2Chapters()
        {
            return new CourseBuilder()
                  .WithCourseId(2)
                  .AddChapter(new ChapterBuilder()
                      .WithChapterId(3)
                      .WithLessonsWithIds(new List<int> { 30, 31, 32 })
                      .Build())
                  .AddChapter(new ChapterBuilder()
                      .WithChapterId(4)
                      .WithLessonsWithIds(new List<int> { 40, 41 })
                      .Build())
                  .Build();
        }
    }
}