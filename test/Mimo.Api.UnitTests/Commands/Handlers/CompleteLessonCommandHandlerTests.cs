using AutoMapper;
using Mimo.Api.Commands;
using Mimo.Api.Commands.Handlers;
using Mimo.Api.Dtos;
using Mimo.Api.Messages;
using Mimo.Api.UnitTests.Builders;
using Mimo.Api.Utils;
using Mimo.Api.Validators;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mimo.Api.UnitTests.Commands.Handlers
{
    public class CompleteLessonCommandHandlerTests
    {
        private readonly Mock<IMimoDbContext> dbContextMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IUserAchievementsUpdater> updaterMock;
        private readonly Mock<IAchievementTypesToUpdateCalculator> calculatorMock;
        private readonly Mock<IResultValidator<CompleteLessonCommand>> validatorMock;

        private readonly CompleteLessonCommandHandler handler;
        private CompleteLessonCommand command;
        private const int lessonId = 33;

        public CompleteLessonCommandHandlerTests()
        {
            command = new CompleteLessonCommand
            {
                Username = "test",
                LessonId = lessonId,
                StartTime = DateTime.UtcNow.AddMinutes(-30),
                EndTime = DateTime.UtcNow
            };
            dbContextMock = new Mock<IMimoDbContext>();
            var achievementsDbSetMock = Helpers.GetMockDbSet(new List<Achievement> { new Achievement() }.AsQueryable());
            dbContextMock.SetupGet(x => x.Achievements)
                .Returns(achievementsDbSetMock.Object);
            mapperMock = new Mock<IMapper>();
            updaterMock = new Mock<IUserAchievementsUpdater>();
            calculatorMock = new Mock<IAchievementTypesToUpdateCalculator>();
            validatorMock = new Mock<IResultValidator<CompleteLessonCommand>>();
            validatorMock.Setup(x => x.Validate(It.IsAny<CompleteLessonCommand>()))
                .Returns(Result.Ok());
            handler = new CompleteLessonCommandHandler(dbContextMock.Object,
                mapperMock.Object,
                updaterMock.Object,
                calculatorMock.Object,
                validatorMock.Object);
        }

        [Fact]
        public void ForLessonAlreadyCompleted_DoesNotModifyAchievements()
        {
            //arrange
            var user = new UserBuilder()
                .WithUsername(command.Username)
                .AddCompletedLesson(new UserLesson
                {
                    LessonId = lessonId,
                    ChapterId = 4,
                    CourseId = 3,
                    NoOfCompletions = 2
                })
                .Build();
            SetupDbReturnsUser(user);
            //act
            var result = handler.Handle(command);

            //assert
            Assert.True(result.Success);
            Assert.Empty(result.Value);
            calculatorMock.Verify(x => x.CalculateAchievementTypesToUpdate(It.IsAny<ICollection<UserLesson>>(),
                It.IsAny<Course>(),
                It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public void ForLessonAlreadyCompleted_UpdatesCompletedLesson()
        {
            //arrange
            var user = new UserBuilder()
                .WithUsername(command.Username)
                .AddCompletedLesson(new UserLesson
                {
                    LessonId = lessonId,
                    ChapterId = 4,
                    CourseId = 3,
                    NoOfCompletions = 2,
                    LastStartedOn = DateTime.UtcNow.AddDays(-3),
                    LastFinishedOn = DateTime.UtcNow.AddDays(-2)
                })
                .Build();
            SetupDbReturnsUser(user);

            //act
            var result = handler.Handle(command);

            //assert
            Assert.True(result.Success);
            Assert.Equal(3, user.CompletedLessons.First().NoOfCompletions);
            Assert.Equal(command.StartTime, user.CompletedLessons.First().LastStartedOn);
            Assert.Equal(command.EndTime, user.CompletedLessons.First().LastFinishedOn);
        }

        [Fact]
        public void WhenLessonBelongsToUnpublishedCourse_ReturnsError()
        {
            //arrange
            var user = new UserBuilder()
                .WithUsername(command.Username)
                .AddCompletedLesson(new UserLesson
                {
                    LessonId = 56,//other completed lesson
                    ChapterId = 4,
                    CourseId = 3,
                    NoOfCompletions = 2,
                    LastStartedOn = DateTime.UtcNow.AddDays(-3),
                    LastFinishedOn = DateTime.UtcNow.AddDays(-2)
                })
                .Build();
            SetupDbReturnsUser(user);
            SetupDbReturnsLesson(new LessonBuilder()
                .WithId(lessonId)
                .WithUnpublishedCourse().Build());

            //act
            var result = handler.Handle(command);

            //assert
            Assert.False(result.Success);
            Assert.Equal(Constants.ErrorCodes.CourseNotPublished, result.Errors.First().ErrorCode);
        }

        [Fact]
        public void WhenLessonDoesNotExist_ReturnsError()
        {
            //arrange
            var user = new UserBuilder()
                .WithUsername(command.Username)
                .AddCompletedLesson(new UserLesson
                {
                    LessonId = 56,//other completed lesson
                    ChapterId = 4,
                    CourseId = 3,
                    NoOfCompletions = 2,
                    LastStartedOn = DateTime.UtcNow.AddDays(-3),
                    LastFinishedOn = DateTime.UtcNow.AddDays(-2)
                })
                .Build();
            SetupDbReturnsUser(user);
            SetupDbReturnsLesson(new LessonBuilder()
                .WithId(45)//other lessonId
                .WithUnpublishedCourse().Build());

            //act
            var result = handler.Handle(command);

            //assert
            Assert.False(result.Success);
            Assert.Equal(Constants.ErrorCodes.LessonNotFound, result.Errors.First().ErrorCode);
        }

        [Fact]
        public void AddsNewUserCompletedLesson()
        {
            //arrange
            const int courseId = 45;
            var user = new UserBuilder()
                .WithUsername(command.Username)
                .Build();
            SetupDbReturnsUser(user);
            SetupDbReturnsLesson(new LessonBuilder()
                .WithId(lessonId)
                .WithPublishedCourse(courseId).Build());
            SetupDbReturnsCourse(new CourseBuilder().WithCourseId(courseId).Build());

            //act
            var result = handler.Handle(command);

            //assert
            Assert.True(result.Success);
            Assert.Single(user.CompletedLessons);
            Assert.Equal(lessonId, user.CompletedLessons.First().LessonId);
            Assert.Equal(courseId, user.CompletedLessons.First().CourseId);
        }

        [Fact]
        public void CalsAchievementsTypesToUpdateCalculator()
        {
            //arrange
            const int courseId = 45;
            var user = new UserBuilder()
                .WithUsername(command.Username)
                .Build();
            SetupDbReturnsUser(user);
            SetupDbReturnsLesson(new LessonBuilder()
                .WithId(lessonId)
                .WithPublishedCourse(courseId).Build());
            SetupDbReturnsCourse(new CourseBuilder().WithCourseId(courseId).Build());

            //act
            var result = handler.Handle(command);

            //assert
            Assert.True(result.Success);
            Assert.Single(user.CompletedLessons);
            calculatorMock.Verify(x => x.CalculateAchievementTypesToUpdate(
                It.Is<ICollection<UserLesson>>(p => p == user.CompletedLessons),
                It.Is<Course>(p => p.Id == courseId),
                It.Is<int>(p => p == 1)),//chapterId
                Times.Once);
        }

        [Fact]
        public void UpdatesUserAchievements()
        {
            //arrange
            const int courseId = 45;
            var user = new UserBuilder()
                .WithUsername(command.Username)
                .Build();
            SetupDbReturnsUser(user);
            SetupDbReturnsLesson(new LessonBuilder()
                .WithId(lessonId)
                .WithPublishedCourse(courseId).Build());
            SetupDbReturnsCourse(new CourseBuilder().WithCourseId(courseId).Build());
            var returnedAchievementTypesToUpdate = new AchievementTypesToUpdate { LessonCompleted = true, ChapterCompleted = true };
            SetupAchievementsCalculatorReturns(returnedAchievementTypesToUpdate);
            SetupUpdaterReturnsUpdatedAchievements(new List<UserAchievement>{
                new UserAchievement { Id = 445 }
            });
            //act
            var result = handler.Handle(command);

            //assert
            Assert.True(result.Success);
            calculatorMock.VerifyAll();
            updaterMock.Verify(x => x.UpdateUserAchievements(
                It.Is<User>(p => p == user),
                It.Is<Course>(p => p.Id == courseId),
                It.Is<AchievementTypesToUpdate>(p => p == returnedAchievementTypesToUpdate),
                It.IsAny<ICollection<Achievement>>(),
                It.IsAny<DateTime>())
                , Times.Once);
        }

        [Fact]
        public void SavesChanges()
        {
            //arrange
            const int courseId = 45;
            var user = new UserBuilder()
                .WithUsername(command.Username)
                .Build();
            SetupDbReturnsUser(user);
            SetupDbReturnsLesson(new LessonBuilder()
                .WithId(lessonId)
                .WithPublishedCourse(courseId).Build());
            SetupDbReturnsCourse(new CourseBuilder().WithCourseId(courseId).Build());
            var returnedAchievementTypesToUpdate = new AchievementTypesToUpdate { LessonCompleted = true, ChapterCompleted = true };
            SetupAchievementsCalculatorReturns(returnedAchievementTypesToUpdate);
            SetupUpdaterReturnsUpdatedAchievements(new List<UserAchievement>{
                new UserAchievement { Id = 445 }
            });
            //act
            var result = handler.Handle(command);

            //assert
            Assert.True(result.Success);
            calculatorMock.VerifyAll();
            updaterMock.VerifyAll();
            dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void ReturnesUpdatedAchievements()
        {
            //arrange
            const int courseId = 45;
            var user = new UserBuilder()
                .WithUsername(command.Username)
                .Build();
            SetupDbReturnsUser(user);
            SetupDbReturnsLesson(new LessonBuilder()
                .WithId(lessonId)
                .WithPublishedCourse(courseId).Build());
            SetupDbReturnsCourse(new CourseBuilder().WithCourseId(courseId).Build());
            var returnedAchievementTypesToUpdate = new AchievementTypesToUpdate { LessonCompleted = true, ChapterCompleted = true };
            SetupAchievementsCalculatorReturns(returnedAchievementTypesToUpdate);
            var updatedAchievements = new List<UserAchievement>
            {
                new UserAchievement { Id = 445 }
            };
            SetupUpdaterReturnsUpdatedAchievements(updatedAchievements);
            var mappedAchievements = new List<UserAchievementDto> { new UserAchievementDto { AchievementId = 445 } };
            mapperMock.Setup(x => x.Map<List<UserAchievementDto>>(It.Is<List<UserAchievement>>(p => p == updatedAchievements)))
                .Returns(mappedAchievements)
                .Verifiable();

            //act
            var result = handler.Handle(command);

            //assert
            Assert.True(result.Success);
            Assert.Equal(mappedAchievements, result.Value);
            calculatorMock.VerifyAll();
            updaterMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        private void SetupDbReturnsUser(User user)
        {
            var users = new List<User> { user };
            var mockUsersDbSet = Helpers.GetMockDbSet(users.AsQueryable());
            dbContextMock.SetupGet(x => x.Users)
                .Returns(mockUsersDbSet.Object);
        }

        private void SetupDbReturnsLesson(Lesson lesson)
        {
            var mockDbSet = Helpers.GetMockDbSet(new List<Lesson> { lesson }.AsQueryable());
            dbContextMock.SetupGet(x => x.Lessons)
                .Returns(mockDbSet.Object);
        }

        private void SetupDbReturnsCourse(Course course)
        {
            var mockDbSet = Helpers.GetMockDbSet(new List<Course> { course }.AsQueryable());
            dbContextMock.SetupGet(x => x.Courses)
                .Returns(mockDbSet.Object);
        }

        private void SetupAchievementsCalculatorReturns(AchievementTypesToUpdate toUpdate)
        {
            calculatorMock.Setup(x => x.CalculateAchievementTypesToUpdate(It.IsAny<ICollection<UserLesson>>(),
                It.IsAny<Course>(),
                It.IsAny<int>()))
                .Returns(toUpdate)
                .Verifiable();
        }

        private void SetupUpdaterReturnsUpdatedAchievements(List<UserAchievement> updated)
        {
            updaterMock.Setup(x => x.UpdateUserAchievements(It.IsAny<User>(),
                It.IsAny<Course>(),
                It.IsAny<AchievementTypesToUpdate>(),
                It.IsAny<ICollection<Achievement>>(),
                It.IsAny<DateTime>()))
                .Returns(updated)
                .Verifiable();
        }
    }
}
