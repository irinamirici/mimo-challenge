using Microsoft.EntityFrameworkCore;
using Mimo.Api.Dtos;
using Mimo.Api.Messages;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System.Linq;

namespace Mimo.Api.Queries.Handlers
{
    public class GetCourseQueryHandler : IQueryHandler<GetCourseQuery, Course>
    {
        private readonly IMimoDbContext dbContext;

        public GetCourseQueryHandler(IMimoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Result<Course> Handle(GetCourseQuery query)
        {
            var qry = dbContext.Courses.AsQueryable();
            if (query.IncludeChapters)
            {
                qry = qry.Include(x => x.Chapters);
            }
            var course = qry.FirstOrDefault(x => x.Id == query.CourseId);
            if (course == null)
            {
                return Result.Fail<Course>(new ResultError(Constants.ErrorCodes.CourseNotFound,
                      ErrorMessages.CourseNotFound,
                      null,
                      query.CourseId),
                      System.Net.HttpStatusCode.NotFound);
            }
            if (!query.AllowPublishedCourse && course.IsPublished)
            {
                return Result.Fail<Course>(new ResultError(Constants.ErrorCodes.CourseAlreadyPublished,
                    ErrorMessages.CourseAlreadyPublished,
                    null,
                    query.CourseId));
            }
            return Result.Ok(course);
        }
    }
}
