using AutoMapper;
using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;

namespace Mimo.Api.Commands.Handlers
{
    public class UpdateChapterCommandHandler : ICommandHandler<UpdateChapterCommand, ChapterDto>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IQueryHandler<GetChapterQuery, Chapter> getChapterQueryHandler;
        private readonly IQueryHandler<ChapterNameIsUniqueQuery, bool> uniqueChapterNameQueryHandler;

        public UpdateChapterCommandHandler(IMimoDbContext dbContext,
            IMapper mapper,
            IQueryHandler<GetChapterQuery, Chapter> getChapterQueryHandler,
            IQueryHandler<ChapterNameIsUniqueQuery, bool> uniqueChapterNameQueryHandler)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.getChapterQueryHandler = getChapterQueryHandler;
            this.uniqueChapterNameQueryHandler = uniqueChapterNameQueryHandler;
        }

        public Result<ChapterDto> Handle(UpdateChapterCommand command)
        {
            var query = new GetChapterQuery(command.ChapterId, allowPublishedCourse: true);
            return getChapterQueryHandler.Handle(query)
                .OnSuccess((chapter) => uniqueChapterNameQueryHandler.Handle(
                    new ChapterNameIsUniqueQuery(chapter.Course.Id, command.ChapterId, command.Name))
                    .OnSuccess(() =>
                    {
                        chapter = mapper.Map(command, chapter);
                        dbContext.SaveChanges();

                        return Result.Ok(mapper.Map<ChapterDto>(chapter));
                    }));
        }
    }
}