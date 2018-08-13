using Mimo.Api.Dtos;
using Mimo.Api.Messages;
using Mimo.Persistence.DbContexts;
using System.Linq;

namespace Mimo.Api.Queries.Handlers
{
    public class ChapterNameIsUniqueQueryHandler : IQueryHandler<ChapterNameIsUniqueQuery, bool>
    {
        private readonly IMimoDbContext dbContext;
        public ChapterNameIsUniqueQueryHandler(IMimoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Result<bool> Handle(ChapterNameIsUniqueQuery query)
        {
            var isUnique = false == dbContext.Chapters
                .Any(x => x.Name.ToLower() == query.ChapterName.ToLower() && x.Course.Id == query.CourseId && x.Id != query.ChapterId);
            return (isUnique)
                 ? Result.Ok(isUnique)
                 : Result.Fail<bool>(new ResultError(Constants.ErrorCodes.Duplicate,
                    ErrorMessages.ChapterNameUnique,
                    "Name",
                    query.ChapterName),
                System.Net.HttpStatusCode.Conflict);
        }
    }
}