using HackathonX.DB.Model;
using HackathonX.DB.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HackathonX.DB.Test
{
    public class UserRepositoryTest
    {
        [Fact]
        public async Task GetOrAddUser_NewName_SavesToDb()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();

            var dbContext = new HackathonXContext();
            var userBefore = UserDbHelper(dbContext, name);
            Assert.Null(userBefore);

            // Act
            using var userRepository = new UserRepository(dbContext);
            User result = await userRepository.GetOrAddUser(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            
            var userAfter = UserDbHelper(dbContext, name);
            Assert.Null(userBefore);
        }

        [Fact]
        public async Task GetOrAddUser_ExistingName_GetsFromDb()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();

            var dbContext = new HackathonXContext();
            var userBefore = UserDbHelper(dbContext, name);
            Assert.NotNull(userBefore);

            // Act
            using var userRepository = new UserRepository(new HackathonXContext());
            User result = await userRepository.GetOrAddUser(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
        }

        private static async Task<User?> UserDbHelper(HackathonXContext dbContext, string name)
        {
            return await dbContext.Users.SingleOrDefaultAsync(u => u.Name == name);
        }
    }
}