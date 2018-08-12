using AutoMapper;
using Mimo.Api.Dtos;
using Mimo.Persistence.DbContexts;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Api.Queries.Handlers
{
    public class GetAllCoursesQueryHandler : IQueryHandler<GetAllCoursesQuery, List<CourseDto>>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IMapper mapper;
        public GetAllCoursesQueryHandler(IMimoDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Result<List<CourseDto>> Handle(GetAllCoursesQuery query)
        {
            var qry = dbContext.Courses.AsQueryable();
            if (query.GetOnlyPublishedCourses)
            {
                qry = qry.Where(x => x.IsPublished == true);
            }
            return Result.Ok(mapper.Map<List<CourseDto>>(qry));
        }
    }
}
