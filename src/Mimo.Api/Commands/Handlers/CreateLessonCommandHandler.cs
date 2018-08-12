using AutoMapper;
using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;

namespace Mimo.Api.Commands.Handlers
{
    public class CreateLessonCommandHandler : ICommandHandler<CreateLessonCommand, LessonDto>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IQueryHandler<GetChapterQuery, Chapter> unpublishedChapterQueryHandler;
        private readonly IQueryHandler<LessonNameIsUniqueQuery, bool> uniqueLessonNameQueryHandler;
        private readonly IMapper mapper;

        public CreateLessonCommandHandler(IMimoDbContext dbContext,
         IQueryHandler<GetChapterQuery, Chapter> unpublishedChapterQueryHandler,
         IQueryHandler<LessonNameIsUniqueQuery, bool> uniqueLessonNameQueryHandler,
         IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unpublishedChapterQueryHandler = unpublishedChapterQueryHandler;
            this.uniqueLessonNameQueryHandler = uniqueLessonNameQueryHandler;
            this.mapper = mapper;
        }

        public Result<LessonDto> Handle(CreateLessonCommand command)
        {
            return uniqueLessonNameQueryHandler.Handle(new LessonNameIsUniqueQuery(0 ,command.ChapterId, command.Name))
                .OnSuccess(() => unpublishedChapterQueryHandler.Handle(new GetChapterQuery(command.ChapterId,
                    allowPublishedCourse: false,
                    includeLessons: true))
                    .OnSuccess((chapter) => AddLesson(chapter, command)));
        }
        private Result<LessonDto> AddLesson(Chapter chapter, CreateLessonCommand command)
        {
            var lesson = mapper.Map<Lesson>(command);
            chapter.Lessons.Add(lesson);
            dbContext.SaveChanges();
            return Result.Ok(mapper.Map<LessonDto>(lesson));
        }
    }
}
