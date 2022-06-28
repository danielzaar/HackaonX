using HackathonX.DB.Model;
using HackathonX.DB.Utils;
using Microsoft.EntityFrameworkCore;

namespace HackathonX.DB.Repositories
{
    public class LeaderboardRepository : IDisposable
    {
        private readonly HackathonXContext m_DbContext;

        public LeaderboardRepository(HackathonXContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public void Dispose()
        {
            m_DbContext.Dispose();
        }

        public async Task<IEnumerable<Leaderboard>> GetLeaderboard(int takeTop = 10)
        { 
            IEnumerable<Leaderboard> leaderboard = await m_DbContext.Leaderboards
                .Include(x => x.User)
                .GroupBy(x => x.UserId)
                .Select(x => x.OrderByDescending(x => x.Score).First())
                //.Distinct(new LeaderboardComparer())
                .Take(takeTop)
                //.OrderByDescending(x => x.Score)
                .ToListAsync();
            return leaderboard;
        }

        public async Task SaveUserScore(Guid userId, int score, TimeSpan timeSpent)
        {
            m_DbContext.Leaderboards.Add(
                new Leaderboard
                {
                    UserId = userId,
                    Score = score,
                    Time = timeSpent.Ticks
                });
            await m_DbContext.SaveChangesAsync();
        }
    }
}
