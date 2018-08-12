namespace Mimo.Api.Queries
{
    public class UserAchievementsQuery
    {
        public int UserId { get; }

        public UserAchievementsQuery(int userId)
        {
            UserId = userId;
        }
    }
}
