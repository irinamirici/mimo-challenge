using AutoMapper;
using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;

namespace Mimo.Api.Commands.Handlers
{
    public class CreateChapterCommandHandler : ICommandHandler<CreateChapterCommand, ChapterDto>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IQueryHandler<GetCourseQuery, Course> unpublishedCourseQueryHandler;
        private readonly IQueryHandler<ChapterNameIsUniqueQuery, bool> uniqueChapterNameQueryHandler;
        private readonly IMapper mapper;

        public CreateChapterCommandHandler(IMimoDbContext dbContext,
            IQueryHandler<GetCourseQuery, Course> unpublishedCourseQueryHandler,
            IQueryHandler<ChapterNameIsUniqueQuery, bool> uniqueChapterNameQueryHandler,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unpublishedCourseQueryHandler = unpublishedCourseQueryHandler;
            this.uniqueChapterNameQueryHandler = uniqueChapterNameQueryHandler;
            this.mapper = mapper;
        }

        public Result<ChapterDto> Handle(CreateChapterCommand command)
        {
            return uniqueChapterNameQueryHandler.Handle(new ChapterNameIsUniqueQuery(command.CourseId, 0, command.Name))
                .OnSuccess(() => unpublishedCourseQueryHandler.Handle(
                    new GetCourseQuery(command.CourseId, allowPublishedCourse: false, includeChapters: true))
                    .OnSuccess((course) => AddChapter(course, command)));
        }

        private Result<ChapterDto> AddChapter(Course course, CreateChapterCommand command)
        {
            var chapter = mapper.Map<Chapter>(command);
            course.Chapters.Add(chapter);
            dbContext.SaveChanges();
            return Result.Ok(mapper.Map<ChapterDto>(chapter));
        }
    }
}
