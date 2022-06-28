using HackathonX.DB.Model;
using Microsoft.EntityFrameworkCore;

namespace HackathonX.DB.Repositories
{
    public class LeaderboardRepository : IDisposable
    {
        private HackathonXContext m_DbContext;

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
            IEnumerable<Leaderboard> leaderboard = await m_DbContext.Leaderboards.Take(takeTop).ToListAsync();
            return leaderboard;
        }
    }
}
