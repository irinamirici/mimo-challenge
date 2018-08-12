using AutoMapper;
using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;

namespace Mimo.Api.Commands.Handlers
{
    public class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand, CourseDto>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IQueryHandler<CourseNameIsUniqueQuery, bool> uniqueCourseNameHandler;

        public CreateCourseCommandHandler(IMimoDbContext dbContext,
            IMapper mapper,
            IQueryHandler<CourseNameIsUniqueQuery, bool> uniqueCourseNameHandler)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.uniqueCourseNameHandler = uniqueCourseNameHandler;
        }

        public Result<CourseDto> Handle(CreateCourseCommand command)
        {
            return uniqueCourseNameHandler.Handle(new CourseNameIsUniqueQuery(0, command.Name))
                    .OnSuccess(() => SaveCourse(command));
        }

        private Result<CourseDto> SaveCourse(CreateCourseCommand command)
        {
            var course = mapper.Map<Course>(command);
            dbContext.Courses.Add(course);
            dbContext.SaveChanges();

            return Result.Ok(mapper.Map<CourseDto>(course));
        }
    }
}
