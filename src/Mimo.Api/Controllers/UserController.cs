using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mimo.Api.Dtos;
using Mimo.Api.Infrastructure;
using Mimo.Api.Infrastructure.Extensions;
using Mimo.Api.Queries;
using Mimo.Api.Queries.Handlers;
using Mimo.Persistence.DbContexts;
using System.Collections.Generic;

namespace Mimo.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMimoDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IQueryHandler<UserAchievementsQuery, List<UserAchievementDto>> userAchievementsQueryHandler;
        private readonly IResponseHandler responseHandler;

        public UserController(IMimoDbContext dbContext,
            IMapper mapper,
            IQueryHandler<UserAchievementsQuery, List<UserAchievementDto>> userAchievementsQueryHandler,
            IResponseHandler responseHandler)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userAchievementsQueryHandler = userAchievementsQueryHandler;
            this.responseHandler = responseHandler;
        }

        [HttpGet("{userId}/achievements")]
        public ActionResult<List<UserAchievementDto>> GetAchievements(int userId)
        {
            return userAchievementsQueryHandler.Handle(new UserAchievementsQuery(userId))
                .OnBoth((result) => responseHandler.GetResponse(result));
        }

        //other endpoints for User CRUD
        //when new user is created, static achievement are added to UserAchievements table
    }
}
