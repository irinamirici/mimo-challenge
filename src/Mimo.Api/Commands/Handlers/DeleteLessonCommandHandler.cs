using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System.Collections.Generic;

namespace Mimo.Api.Commands.Handlers
{
    public class DeleteLessonCommandHandler : ICommandHandler<DeleteLessonCommand, bool>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IQueryHandler<GetLessonQuery, Lesson> lessonOfUnpublishedCourseQueryHandler;

        public DeleteLessonCommandHandler(IMimoDbContext dbContext,
          IQueryHandler<GetLessonQuery, Lesson> lessonOfUnpublishedCourseQueryHandler)
        {
            this.dbContext = dbContext;
            this.lessonOfUnpublishedCourseQueryHandler = lessonOfUnpublishedCourseQueryHandler;
        }

        public Result<bool> Handle(DeleteLessonCommand command)
        {
            return lessonOfUnpublishedCourseQueryHandler.Handle(
                    new GetLessonQuery(command.LessonId, allowPublishedCourse: false))
                .OnSuccess((lesson) =>
                {
                    dbContext.Lessons.Remove(lesson);
                    dbContext.SaveChanges();
                    return Result.Ok(true);
                });
        }
    }
}