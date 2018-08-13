using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mimo.Api.Dtos;
using Mimo.Api.Messages;
using Mimo.Api.Utils;
using Mimo.Api.Validators;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Api.Commands.Handlers
{
    public class CompleteLessonCommandHandler : ICommandHandler<CompleteLessonCommand, List<UserAchievementDto>>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IUserAchievementsUpdater userAchievementsUpdater;
        private readonly IAchievementTypesToUpdateCalculator achievementTypesCalculator;
        private readonly IResultValidator<CompleteLessonCommand> completeLessonValidator;

        public CompleteLessonCommandHandler(IMimoDbContext dbContext,
            IMapper mapper,
            IUserAchievementsUpdater userAchievementsUpdater,
            IAchievementTypesToUpdateCalculator achievementTypesCalculator,
            IResultValidator<CompleteLessonCommand> completeLessonValidator)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userAchievementsUpdater = userAchievementsUpdater;
            this.achievementTypesCalculator = achievementTypesCalculator;
            this.completeLessonValidator = completeLessonValidator;
        }

        public Result<List<UserAchievementDto>> Handle(CompleteLessonCommand command)
        {
            var validationResult = completeLessonValidator.Validate(command);
            if (validationResult.Failure)
            {
                return Result.Fail<List<UserAchievementDto>>(validationResult.Errors);
            }
            var user = dbContext.Users.Include(x => x.CompletedLessons)
                .First(x => x.Username == command.Username);
            var completedLesson = user.CompletedLessons.FirstOrDefault(x => x.LessonId == command.LessonId);
            if (completedLesson != null)
            {
                return UpdateAlreadyCompletedLesson(completedLesson, command);
            }
            var lessonResult = GetLessonOfPublishedCourse(command.LessonId);
            if (lessonResult.Failure)
            {
                return Result.Fail<List<UserAchievementDto>>(lessonResult.Errors, lessonResult.StatusCode);
            }

            var courseId = lessonResult.Value.Chapter.Course.Id;
            var chapterId = lessonResult.Value.Chapter.Id;
            user.AddCompletedLesson(BuildCompletedLesson(command, courseId, chapterId));

            var course = dbContext.Courses.Where(x => x.Id == courseId)
                .Include(x => x.Chapters).ThenInclude(x => x.Lessons)
                .First();

            var achievementTypesToUpdate = achievementTypesCalculator
                .CalculateAchievementTypesToUpdate(user.CompletedLessons, course, chapterId);

            var achievements = dbContext.Achievements.ToList();
            var achievementsUpdatedNow = userAchievementsUpdater.UpdateUserAchievements(user,
                course,
                achievementTypesToUpdate,
                achievements,
                DateTime.UtcNow);

            dbContext.SaveChanges();
            return Result.Ok(mapper.Map<List<UserAchievementDto>>(achievementsUpdatedNow));
        }

        private Result<List<UserAchievementDto>> UpdateAlreadyCompletedLesson(UserLesson completedLesson, CompleteLessonCommand command)
        {
            completedLesson.LastStartedOn = command.StartTime.Value;
            completedLesson.LastFinishedOn = command.EndTime.Value;
            completedLesson.NoOfCompletions++;

            dbContext.SaveChanges();
            return Result.Ok(new List<UserAchievementDto>());
        }

        private UserLesson BuildCompletedLesson(CompleteLessonCommand command, int courseId, int chapterId)
        {
            return new UserLesson
            {
                LastStartedOn = command.StartTime.Value,
                LastFinishedOn = command.EndTime.Value,
                LessonId = command.LessonId,
                ChapterId = chapterId,
                CourseId = courseId,
                NoOfCompletions = 1
            };
        }

        private Result<Lesson> GetLessonOfPublishedCourse(int lessonId)
        {
            var lesson = dbContext.Lessons
                  .Include(x => x.Chapter)
                  .ThenInclude(x => x.Course)
                  .FirstOrDefault(x => x.Id == lessonId);
            if (lesson == null)
            {
                return Result.Fail<Lesson>(new ResultError(Constants.ErrorCodes.LessonNotFound,
                    ErrorMessages.LessonNotFound,
                    null,
                 lessonId));
            }

            if (!lesson.Chapter.Course.IsPublished)
            {
                return Result.Fail<Lesson>(new ResultError(Constants.ErrorCodes.CourseNotPublished,
                    ErrorMessages.CompleteLessonCourseNotPublished,
                    null,
                    lesson.Chapter.Course.Id));
            }
            return Result.Ok(lesson);
        }
    }
}