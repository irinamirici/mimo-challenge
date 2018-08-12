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
using Mimo.Persistence.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Api.Controllers
{
    [Authorize]
    [Route("api/chapters")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICommandHandler<CreateChapterCommand, ChapterDto> createChapterCommandHandler;
        private readonly ICommandHandler<UpdateChapterCommand, ChapterDto> updateChapterCommandHandler;
        private readonly ICommandHandler<DeleteChapterCommand, bool> deleteChapterCommandHandler;
        private readonly IQueryHandler<GetCourseQuery, Course> getCourseQueryHandler;
        private readonly IQueryHandler<GetChapterQuery, Chapter> getChapterQueryHandler;
        private readonly IResponseHandler responseHandler;

        public ChaptersController(IMapper mapper,
            ICommandHandler<CreateChapterCommand, ChapterDto> createChapterCommandHandler,
            ICommandHandler<UpdateChapterCommand, ChapterDto> updateChapterCommandHandler,
            ICommandHandler<DeleteChapterCommand, bool> deleteChapterCommandHandler,
            IQueryHandler<GetCourseQuery, Course> getCourseQueryHandler,
            IQueryHandler<GetChapterQuery, Chapter> getChapterQueryHandler,
            IResponseHandler responseHandler)
        {
            this.mapper = mapper;
            this.createChapterCommandHandler = createChapterCommandHandler;
            this.updateChapterCommandHandler = updateChapterCommandHandler;
            this.deleteChapterCommandHandler = deleteChapterCommandHandler;
            this.getCourseQueryHandler = getCourseQueryHandler;
            this.getChapterQueryHandler = getChapterQueryHandler;
            this.responseHandler = responseHandler;
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpPost]
        public ActionResult<ChapterDto> Create([FromBody] CreateChapterCommand command)
        {
            return createChapterCommandHandler.Handle(command)
                .OnBoth((result) =>
                {
                    return responseHandler.GetCreatedResponse(result, "GetChapter", new
                    {
                        chapterId = result.Value?.Id
                    });
                });
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpPost("{chapterId}")]
        public ActionResult<ChapterDto> Update(int chapterId, [FromBody] UpdateChapterCommand command)
        {
            command.ChapterId = chapterId;
            return updateChapterCommandHandler.Handle(command)
                .OnBoth((result) =>
                {
                    return responseHandler.GetResponse(result);
                });
        }

        [Authorize(Roles = "ContentCreator")]
        [HttpDelete("{chapterId}")]
        public ActionResult Delete(int chapterId)
        {
            var command = new DeleteChapterCommand(chapterId);
            return deleteChapterCommandHandler.Handle(command)
                .OnBoth((result) =>
                {
                    return responseHandler.GetSimpleResponse(result);
                });
        }

        [HttpGet]
        public ActionResult<IEnumerable<ChapterDto>> GetAllByCourse([FromQuery]int courseId)
        {
            var query = new GetCourseQuery(courseId, allowPublishedCourse: true, includeChapters: true);
            var courseResult = getCourseQueryHandler.Handle(query);
            var chapterResult = courseResult.Success
                ? Result.Ok(mapper.Map<IEnumerable<ChapterDto>>(courseResult.Value.Chapters.OrderBy(x=>x.Order)))
                : Result.Fail<IEnumerable<ChapterDto>>(courseResult.Errors, courseResult.StatusCode);

            return responseHandler.GetResponse(chapterResult);
        }

        [HttpGet("{chapterId}", Name = "GetChapter")]
        public ActionResult<ChapterDto> Get(int chapterId, bool includeLessons = false)
        {
            var query = new GetChapterQuery(chapterId, allowPublishedCourse: true, includeLessons: includeLessons);

            return getChapterQueryHandler.Handle(query)
                .OnSuccess((chapter) => MapToDto(chapter, includeLessons))
                   .OnBoth((result) => responseHandler.GetResponse(result));
        }

        private Result<ChapterDto> MapToDto(Chapter chapter, bool includeLessons)
        {
            if (includeLessons)
            {
                var chapterDto = mapper.Map<ChapterWithLessonsDto>(chapter);
                chapterDto.Lessons = chapterDto.Lessons.OrderBy(x => x.Order).ToList();
                return Result.Ok((ChapterDto)chapterDto);
            }
            return Result.Ok(mapper.Map<ChapterDto>(chapter));
        }
    }
}