using Microsoft.EntityFrameworkCore;
using WebSpectre.Client.Data;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(UserModel user, CancellationToken cancellationToken)
        {
            await _dataContext.Users.AddAsync(new User { Username = user.Username, Password = user.Username }, cancellationToken);
        }

        public async Task<User?> GetExistAsync(string username, CancellationToken cancellationToken)
        {
            var exist = await _dataContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
            return exist;
        }

        public async Task<bool> LoginAsync(string username, string passsword, CancellationToken cancellationToken)
        {
            var existUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == passsword, cancellationToken);
            if (existUser != null)
                return true;

            return false;
        }
    }
}
