using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;

namespace Mimo.Api.Commands.Handlers
{
    public class PublishCourseCommandHandler : ICommandHandler<PublishCourseCommand, bool>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IQueryHandler<GetCourseQuery, Course> unpublishedCourseQueryHandler;

        public PublishCourseCommandHandler(IMimoDbContext dbContext,
            IQueryHandler<GetCourseQuery, Course> unpublishedCourseQueryHandler)
        {
            this.dbContext = dbContext;
            this.unpublishedCourseQueryHandler = unpublishedCourseQueryHandler;
        }

        public Result<bool> Handle(PublishCourseCommand command)
        {
            return unpublishedCourseQueryHandler.Handle(new GetCourseQuery(command.CourseId, allowPublishedCourse: false))
                  .OnSuccess((course) =>
                  {
                      course.IsPublished = true;
                      dbContext.SaveChanges();
                      return Result.Ok(true);
                  });
        }
    }
}
