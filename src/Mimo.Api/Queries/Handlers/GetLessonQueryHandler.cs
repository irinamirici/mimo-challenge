using Microsoft.EntityFrameworkCore;
using Mimo.Api.Dtos;
using Mimo.Api.Messages;
using Mimo.Persistence.DbContexts;
using Mimo.Persistence.Entities;
using System.Linq;

namespace Mimo.Api.Queries.Handlers
{
    public class GetLessonQueryHandler : IQueryHandler<GetLessonQuery, Lesson>
    {
        private readonly IMimoDbContext dbContext;

        public GetLessonQueryHandler(IMimoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Result<Lesson> Handle(GetLessonQuery query)
        {
            var lesson = dbContext.Lessons.Include(x => x.Chapter)
                .ThenInclude(x => x.Course)
               .FirstOrDefault(x => x.Id == query.LessonId);

            if (lesson == null)
            {
                return Result.Fail<Lesson>(new ResultError(Constants.ErrorCodes.LessonNotFound,
                    ErrorMessages.LessonNotFound,
                    null,
                    query.LessonId),
                    System.Net.HttpStatusCode.NotFound);
            }

            if (!query.AllowPublishedCourse && lesson.Chapter.Course.IsPublished)
            {
                return Result.Fail<Lesson>(new ResultError(Constants.ErrorCodes.CourseAlreadyPublished,
                   ErrorMessages.CourseAlreadyPublished,
                   null,
                   lesson.Chapter.Course.Id));
            }
            return Result.Ok(lesson);
        }
    }
}