using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mimo.Api.Commands;
using Mimo.Api.Commands.Handlers;
using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Api.Validators;
using Mimo.Persistence.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Api.Controllers
{
    [Authorize]
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICommandHandler<CreateCourseCommand, CourseDto> createCourseCommandHandler;
        private readonly ICommandHandler<UpdateCourseCommand, CourseDto> updateCourseCommandHandler;
        private readonly ICommandHandler<DeleteCourseCommand, bool> deleteCourseCommandHandler;
        private readonly ICommandHandler<PublishCourseCommand, bool> publishCourseCommandHandler;
        private readonly IQueryHandler<GetCourseQuery, Course> getCourseQueryHandler;
        private readonly IQueryHandler<GetAllCoursesQuery, List<CourseDto>> getAllCoursesQueryHandler;
        private readonly IResultValidator<CreateCourseCommand> createCourseValidator;
        private readonly IResponseHandler responseHandler;

        public CoursesController(IMapper mapper,
            ICommandHandler<CreateCourseCommand, CourseDto> createCourseCommandHandler,
            ICommandHandler<UpdateCourseCommand, CourseDto> updateCourseCommandHandler,
            ICommandHandler<DeleteCourseCommand, bool> deleteCourseCommandHandler,
            ICommandHandler<PublishCourseCommand, bool> publishCourseCommandHandler,
            IQueryHandler<GetCourseQuery, Course> getCourseQueryHandler,
            IQueryHandler<GetAllCoursesQuery, List<CourseDto>> getAllCoursesQueryHandler,
            IResultValidator<CreateCourseCommand> createCourseValidator,
            IResponseHandler responseHandler)
        {
            this.mapper = mapper;
            this.createCourseCommandHandler = createCourseCommandHandler;
            this.updateCourseCommandHandler = updateCourseCommandHandler;
            this.deleteCourseCommandHandler = deleteCourseCommandHandler;
            this.publishCourseCommandHandler = publishCourseCommandHandler;
            this.getCourseQueryHandler = getCourseQueryHandler;
            this.getAllCoursesQueryHandler = getAllCoursesQueryHandler;
            this.createCourseValidator = createCourseValidator;
            this.responseHandler = responseHandler;
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpPost]
        public ActionResult<CourseDto> Create([FromBody]CreateCourseCommand command)
        {
            return createCourseValidator.Validate(command)
                 .OnSuccess(() => createCourseCommandHandler.Handle(command))
                     .OnBoth((result) => responseHandler.GetCreatedResponse(result, "GetCourse", new
                     {
                         courseId = result.Value?.Id
                     }));
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpPost("{courseId}")]
        public ActionResult<CourseDto> Update(int courseId, [FromBody]UpdateCourseCommand command)
        {
            command.Id = courseId;
            return updateCourseCommandHandler.Handle(command)
                .OnBoth((result) => responseHandler.GetResponse(result));
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpPost("{courseId}/actions/publish")]
        public ActionResult Publish(int courseId)
        {
            return publishCourseCommandHandler.Handle(new PublishCourseCommand(courseId))
                .OnBoth((result) => responseHandler.GetSimpleResponse(result));
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpDelete("{courseId}")]
        public ActionResult Delete(int courseId)
        {
            return deleteCourseCommandHandler.Handle(new DeleteCourseCommand { CourseId = courseId })
                .OnBoth((result) => responseHandler.GetSimpleResponse(result));
        }

        [HttpGet]
        public ActionResult<List<CourseDto>> GetAll()
        {
            return getAllCoursesQueryHandler.Handle(new GetAllCoursesQuery())
                 .OnBoth((result) => responseHandler.GetResponse(result));
        }

        [HttpGet("{courseId}", Name = "GetCourse")]
        public ActionResult<CourseDto> Get(int courseId, bool includeChapters = false)
        {
            var query = new GetCourseQuery(courseId, allowPublishedCourse: true, includeChapters: includeChapters);

            return getCourseQueryHandler.Handle(query)
                 .OnSuccess((course) => MapToDto(course, includeChapters))
                    .OnBoth((result) => responseHandler.GetResponse(result));
        }

        private Result<CourseDto> MapToDto(Course course, bool includeChapters)
        {
            if (includeChapters)
            {
                var courseDto = mapper.Map<CourseWithChaptersDto>(course);
                courseDto.Chapters = courseDto.Chapters.OrderBy(x => x.Order).ToList();
                return Result.Ok((CourseDto)courseDto);
            }
            return Result.Ok(mapper.Map<CourseDto>(course));
        }
    }
}