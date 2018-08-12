using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System.Collections.Generic;

namespace Mimo.Api.Commands.Handlers
{
    public class DeleteChapterCommandHandler : ICommandHandler<DeleteChapterCommand, bool>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IQueryHandler<GetChapterQuery, Chapter> chapterOfUnpublishedCourseQueryHandler;

        public DeleteChapterCommandHandler(IMimoDbContext dbContext,
          IQueryHandler<GetChapterQuery, Chapter> chapterOfUnpublishedCourseQueryHandler)
        {
            this.dbContext = dbContext;
            this.chapterOfUnpublishedCourseQueryHandler = chapterOfUnpublishedCourseQueryHandler;
        }

        public Result<bool> Handle(DeleteChapterCommand command)
        {
            return chapterOfUnpublishedCourseQueryHandler.Handle(
                    new GetChapterQuery(command.ChapterId, allowPublishedCourse: false))
                .OnSuccess((chapter) =>
                {
                    dbContext.Chapters.Remove(chapter);
                    dbContext.SaveChanges();
                    return Result.Ok(true);
                });
        }
    }
}