using Microsoft.EntityFrameworkCore;
using Mimo.Api.Dtos;
using Mimo.Api.Messages;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System.Linq;

namespace Mimo.Api.Queries.Handlers
{
    public class GetChapterQueryHandler : IQueryHandler<GetChapterQuery, Chapter>
    {
        private readonly IMimoDbContext dbContext;

        public GetChapterQueryHandler(IMimoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Result<Chapter> Handle(GetChapterQuery query)
        {
            var qry = dbContext.Chapters.Include(x => x.Course).AsQueryable();
            if (query.IncludeLessons)
            {
                qry = qry.Include(x => x.Lessons);
            }
            var chapter = qry.FirstOrDefault(x => x.Id == query.ChapterId);

            if (chapter == null)
            {
                return Result.Fail<Chapter>(new ResultError(Constants.ErrorCodes.ChapterNotFound,
                    ErrorMessages.ChapterNotFound,
                    null,
                    query.ChapterId),
                    System.Net.HttpStatusCode.NotFound);
            }

            if (!query.AllowPublishedCourse && chapter.Course.IsPublished)
            {
                return Result.Fail<Chapter>(new ResultError(Constants.ErrorCodes.CourseAlreadyPublished,
                   ErrorMessages.CourseAlreadyPublished,
                   null,
                   chapter.Course.Id));
            }
            return Result.Ok(chapter);
        }
    }
}
