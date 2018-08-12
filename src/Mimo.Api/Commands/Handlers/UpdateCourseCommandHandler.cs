using AutoMapper;
using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Messages;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System.Linq;

namespace Mimo.Api.Commands.Handlers
{
    public class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand, CourseDto>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IQueryHandler<CourseNameIsUniqueQuery, bool> uniqueCourseNameHandler;

        public UpdateCourseCommandHandler(IMimoDbContext dbContext,
            IMapper mapper,
            IQueryHandler<CourseNameIsUniqueQuery, bool> uniqueCourseNameHandler)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.uniqueCourseNameHandler = uniqueCourseNameHandler;
        }

        public Result<CourseDto> Handle(UpdateCourseCommand command)
        {
            return GetCourseById(command.Id)
                   .OnSuccess((course) => uniqueCourseNameHandler.Handle(new CourseNameIsUniqueQuery(command.Id, command.Name))
                       .OnSuccess(() =>
                       {
                           course = mapper.Map(command, course);

                           dbContext.SaveChanges();
                           return Result.Ok(mapper.Map<CourseDto>(course));
                       }));
        }

        private Result<Course> GetCourseById(int courseId)
        {
            var course = dbContext.Courses
              .FirstOrDefault(x => x.Id == courseId);
            if (course == null)
            {
                return Result.Fail<Course>(new ResultError(Constants.ErrorCodes.CourseNotFound,
                    ErrorMessages.CourseNotFound,
                    null,
                    courseId),
                    System.Net.HttpStatusCode.NotFound);
            }
            return Result.Ok(course);
        }
    }
}
