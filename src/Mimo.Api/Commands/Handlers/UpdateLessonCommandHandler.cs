using AutoMapper;
using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;

namespace Mimo.Api.Commands.Handlers
{
    public class UpdateLessonCommandHandler : ICommandHandler<UpdateLessonCommand, LessonDto>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IQueryHandler<GetLessonQuery, Lesson> getLessonQueryHandler;
        private readonly IQueryHandler<LessonNameIsUniqueQuery, bool> uniqueLessonNameQueryHandler;

        public UpdateLessonCommandHandler(IMimoDbContext dbContext,
            IMapper mapper,
            IQueryHandler<GetLessonQuery, Lesson> getLessonQueryHandler,
            IQueryHandler<LessonNameIsUniqueQuery, bool> uniqueLessonNameQueryHandler)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.getLessonQueryHandler = getLessonQueryHandler;
            this.uniqueLessonNameQueryHandler = uniqueLessonNameQueryHandler;
        }

        public Result<LessonDto> Handle(UpdateLessonCommand command)
        {
            var query = new GetLessonQuery(command.LessonId, allowPublishedCourse: true);
            return getLessonQueryHandler.Handle(query)
                .OnSuccess((lesson) => uniqueLessonNameQueryHandler.Handle(
                    new LessonNameIsUniqueQuery(command.LessonId, lesson.Chapter.Id, command.Name))
                    .OnSuccess(() =>
                    {
                        lesson = mapper.Map(command, lesson);
                        dbContext.SaveChanges();

                        return Result.Ok(mapper.Map<LessonDto>(lesson));
                    }));
        }
    }
}
