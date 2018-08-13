using Mimo.Api.Dtos;
using Mimo.Api.Messages;
using Mimo.Persistence.DbContexts;
using System.Linq;

namespace Mimo.Api.Queries.Handlers
{
    public class LessonNameIsUniqueQueryHandler : IQueryHandler<LessonNameIsUniqueQuery, bool>
    {
        private readonly IMimoDbContext dbContext;
        public LessonNameIsUniqueQueryHandler(IMimoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Result<bool> Handle(LessonNameIsUniqueQuery query)
        {
            var isUnique = false == dbContext.Lessons
                   .Any(x => x.Name.ToLower() == query.LessonName.ToLower() && x.Chapter.Id == query.ChapterId && x.Id != query.LessonId);
            return (isUnique)
                 ? Result.Ok(isUnique)
                 : Result.Fail<bool>(new ResultError(Constants.ErrorCodes.Duplicate,
                    ErrorMessages.LessonNameUnique,
                    "Name",
                    query.LessonName),
                System.Net.HttpStatusCode.Conflict);
        }
    }
}