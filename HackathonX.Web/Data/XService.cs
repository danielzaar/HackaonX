using HackathonX.DB.Model;
using HackathonX.DB.Repositories;

namespace HackathonX.Web.Data
{
    public class XService
    {
        private readonly HackathonXContext _dbContext;

        public XService(HackathonXContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetOrCreateUser(string name)
        {
            using var userRepo = new UserRepository(_dbContext);
            var user = await userRepo.GetOrAddUser(name);
            return new User { Id = user.Id, Name = user.Name };
        }

        //public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        //{
        //    return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = startDate.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    }).ToArray());
        //}
    }
}