using Mimo.Api.Dtos;
using Mimo.Api.Messages;
using Mimo.Persistence.DbContexts;
using System.Linq;

namespace Mimo.Api.Queries.Handlers
{
    public class CourseNameIsUniqueQueryHandler : IQueryHandler<CourseNameIsUniqueQuery, bool>
    {
        private readonly IMimoDbContext dbContext;
        public CourseNameIsUniqueQueryHandler(IMimoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Result<bool> Handle(CourseNameIsUniqueQuery query)
        {
            var isUnique = false == dbContext.Courses.Any(x => x.Name.ToLower() == query.Name.ToLower() && x.Id != query.CourseId);
            return (isUnique)
                 ? Result.Ok(isUnique)
                 : Result.Fail<bool>(new ResultError(Constants.ErrorCodes.Duplicate, ErrorMessages.CourseNameUnique, "Name", query.Name),
                     System.Net.HttpStatusCode.Conflict);
        }
    }
}