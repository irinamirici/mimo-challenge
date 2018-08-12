using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;

namespace Mimo.Api.Commands.Handlers
{
    public class DeleteCourseCommandHandler : ICommandHandler<DeleteCourseCommand, bool>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IQueryHandler<GetCourseQuery, Course> unpublishedCourseQueryHandler;

        public DeleteCourseCommandHandler(IMimoDbContext dbContext,
          IQueryHandler<GetCourseQuery, Course> unpublishedCourseQueryHandler)
        {
            this.dbContext = dbContext;
            this.unpublishedCourseQueryHandler = unpublishedCourseQueryHandler;
        }

        public Result<bool> Handle(DeleteCourseCommand command)
        {
            return unpublishedCourseQueryHandler.Handle(new GetCourseQuery(command.CourseId, allowPublishedCourse: false))
                    .OnSuccess((course) =>
                    {
                        dbContext.Courses.Remove(course);
                        dbContext.SaveChanges();
                        return Result.Ok(true);
                    });

        }
    }
}