using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mimo.Api.Commands;
using Mimo.Api.Commands.Handlers;
using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Api.Controllers
{
    [Authorize]
    [Route("api/lessons")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICommandHandler<CompleteLessonCommand, List<UserAchievementDto>> completeLessonHandler;
        private readonly ICommandHandler<CreateLessonCommand, LessonDto> createLessonHandler;
        private readonly ICommandHandler<UpdateLessonCommand, LessonDto> updateLessonHandler;
        private readonly ICommandHandler<DeleteLessonCommand, bool> deleteLessonHandler;

        private readonly IQueryHandler<GetLessonQuery, Lesson> getLessonQueryHandler;
        private readonly IQueryHandler<GetChapterQuery, Chapter> getChapterQueryHandler;
        private readonly IResponseHandler responseHandler;
        private readonly IHttpContextAccessor contextAccesor;

        public LessonsController(IMapper mapper,
            ICommandHandler<CompleteLessonCommand, List<UserAchievementDto>> completeLessonHandler,
            ICommandHandler<CreateLessonCommand, LessonDto> createLessonHandler,
            ICommandHandler<UpdateLessonCommand, LessonDto> updateLessonHandler,
            ICommandHandler<DeleteLessonCommand, bool> deleteLessonHandler,
            IQueryHandler<GetLessonQuery, Lesson> getLessonQueryHandler,
            IQueryHandler<GetChapterQuery, Chapter> getChapterQueryHandler,
            IResponseHandler responseHandler,
            IHttpContextAccessor contextAccesor)
        {
            this.mapper = mapper;
            this.completeLessonHandler = completeLessonHandler;
            this.createLessonHandler = createLessonHandler;
            this.updateLessonHandler = updateLessonHandler;
            this.deleteLessonHandler = deleteLessonHandler;
            this.getLessonQueryHandler = getLessonQueryHandler;
            this.getChapterQueryHandler = getChapterQueryHandler;
            this.responseHandler = responseHandler;
            this.contextAccesor = contextAccesor;
        }

        [Authorize(Roles = "Client")]
        [HttpPost("{lessonId}/actions/complete")]
        public ActionResult<List<UserAchievementDto>> CompleteLesson(int lessonId, [FromBody] CompleteLessonCommand command)
        {
            command.LessonId = lessonId;
            command.Username = contextAccesor.HttpContext.User.Identity.Name;
            return  completeLessonHandler.Handle(command)
                .OnBoth((result) => responseHandler.GetResponse(result));
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpPost]
        public ActionResult<LessonDto> Create([FromBody] CreateLessonCommand command)
        {
            return createLessonHandler.Handle(command)
                .OnBoth((result) =>
                {
                    return responseHandler.GetCreatedResponse(result, "GetLesson", new
                    {
                        lessonId = result.Value?.Id
                    });
                });
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpPost("{lessonId}")]
        public ActionResult<LessonDto> Update(int lessonId, [FromBody] UpdateLessonCommand command)
        {
            command.LessonId = lessonId;
            return updateLessonHandler.Handle(command)
                .OnBoth((result) =>
                {
                    return responseHandler.GetResponse(result);
                });
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpDelete("{lessonId}")]
        public ActionResult Delete(int lessonId)
        {
            var command = new DeleteLessonCommand(lessonId);
            return deleteLessonHandler.Handle(command)
                .OnBoth((result) =>
                {
                    return responseHandler.GetSimpleResponse(result);
                });
        }

        [HttpGet]
        public ActionResult<IEnumerable<LessonDto>> GetAllByChapter([FromQuery]int chapterId)
        {
            var query = new GetChapterQuery(chapterId, allowPublishedCourse: true, includeLessons: true);
            var chapterResult = getChapterQueryHandler.Handle(query);
            var lessonResult = chapterResult.Success
                ? Result.Ok(mapper.Map<IEnumerable<LessonDto>>(chapterResult.Value.Lessons.OrderBy(x => x.Order)))
                : Result.Fail<IEnumerable<LessonDto>>(chapterResult.Errors, chapterResult.StatusCode);

            return responseHandler.GetResponse(lessonResult);
        }

        [HttpGet("{lessonId}", Name = "GetLesson")]
        public ActionResult<LessonDto> Get(int lessonId)
        {
            return getLessonQueryHandler.Handle(new GetLessonQuery(lessonId))
                    .OnSuccess((lesson) =>
                    {
                        return Result.Ok(mapper.Map<LessonDto>(lesson));
                    })
                    .OnBoth((result) => responseHandler.GetResponse(result));
        }
    }
}
