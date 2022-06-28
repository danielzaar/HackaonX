using HackathonX.DB.Model;
using HackathonX.DB.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HackathonX.DB.Test
{
    public class LeaderboardRepositoryTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<HackathonXContext> _contextOptions;
        private readonly HackathonXContext _hackathonXContext;

        public LeaderboardRepositoryTest()
        {
            _connection = new SqliteConnection("Data Source=HackathonX.db;");//new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<HackathonXContext>()
                .UseSqlite(_connection)
                .Options;

            _hackathonXContext = new HackathonXContext(_contextOptions);
        }

        public void Dispose()
        {
            _hackathonXContext?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }

        [Fact]
        public async Task SaveUserScore_Success()
        {
            // Arrange
            int score = 123;
            TimeSpan time = TimeSpan.FromMinutes(8);
            string userName = Guid.NewGuid().ToString("N");
            var user = await AddUserToDbHelper(_hackathonXContext, userName);

            // Act
            using var leaderboardRepository = new LeaderboardRepository(_hackathonXContext);
            await leaderboardRepository.SaveUserScore(user.Id, score, time);

            // Assert
            var leaderboard = await GetLeaderboardFromDbHelper(_hackathonXContext, user.Id);
            Assert.NotNull(leaderboard);
            Assert.Equal(score, leaderboard?.Score);
            Assert.Equal(time.Ticks, leaderboard?.Time);
        }

        [Fact]
        public async Task GetLeaderboard_TakeTen_ReturnsCorrectData()
        {
            // Arrange
            string repetitiveName = Guid.NewGuid().ToString("N");
            for (int i = 0; i < 5; i++)
            {
                string userName = Guid.NewGuid().ToString("N");
                await AddLeadersToDbHelper(_hackathonXContext, userName, (i + 1) * 100, i + 5);
                await AddLeadersToDbHelper(_hackathonXContext, repetitiveName, (i + 1) * 50, i + 7);
            }

            // Act
            using var leaderboardRepository = new LeaderboardRepository(_hackathonXContext);
            IEnumerable<Leaderboard> result = await leaderboardRepository.GetLeaderboard(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
            Assert.True(result.All(x => x.User != null));
            Assert.Single(result.Where(x => x.User.Name == repetitiveName));
            Assert.Equal(500, result.First().Score);
            Assert.Equal(TimeSpan.FromMinutes(9).Ticks, result.First().Time);
        }

        private static async Task<User> AddUserToDbHelper(HackathonXContext dbContext, string name)
        {
            var user = dbContext.Users.Add(new User { Name = name }).Entity;
            await dbContext.SaveChangesAsync();

            return user;
        }

        private static async Task<Leaderboard?> GetLeaderboardFromDbHelper(HackathonXContext dbContext, Guid userId)
        {
            return await dbContext.Leaderboards.SingleOrDefaultAsync(x => x.UserId == userId);
        }

        private static async Task AddLeadersToDbHelper(HackathonXContext dbContext, string name, int score, int time)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Name == name);
            if (user == null)
            {
                user = dbContext.Users.Add(new User { Name = name }).Entity;
            }

            dbContext.Leaderboards.Add(
                new Leaderboard
                {
                    UserId = user.Id,
                    Score = score,
                    Time = TimeSpan.FromMinutes(time).Ticks,
                    Timestamp = DateTime.Now
                });

            await dbContext.SaveChangesAsync();
        }
    }
}