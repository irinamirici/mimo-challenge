using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mimo.Api.Dtos;
using Mimo.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimo.Api.Queries.Handlers
{
    public class UserAchievementsQueryHandler : IQueryHandler<UserAchievementsQuery, List<UserAchievementDto>>
    {
        private readonly IMimoDbContext dbContext;
        private readonly IMapper mapper;

        public UserAchievementsQueryHandler(IMimoDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        public Result<List<UserAchievementDto>> Handle(UserAchievementsQuery query)
        {
            var achievements = dbContext.Users
                .Include(x => x.Achievements)
                .ThenInclude(x => x.Course)
                .Where(x => x.Id == query.UserId)
                .SelectMany(x => x.Achievements).ToList();
            return Result.Ok(mapper.Map<List<UserAchievementDto>>(achievements));
        }
    }
}
