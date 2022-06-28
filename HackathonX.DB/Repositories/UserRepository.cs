using HackathonX.DB.Model;
using Microsoft.EntityFrameworkCore;

namespace HackathonX.DB.Repositories
{
    public class UserRepository : IDisposable
    {
        private HackathonXContext m_DbContext;

        public UserRepository(HackathonXContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public void Dispose()
        {
            m_DbContext.Dispose();
        }

        public async Task<User> GetOrAddUser(string name)
        { 
            User? user = await m_DbContext.Users.SingleOrDefaultAsync(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                user = m_DbContext.Users.Add(new User { Name = name }).Entity;
                await m_DbContext.SaveChangesAsync();
            }
            return user;
        }
    }
}
