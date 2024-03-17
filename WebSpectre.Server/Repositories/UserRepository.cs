using Microsoft.EntityFrameworkCore;
using WebSpectre.Server.Data;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Repositories
{
    public class UserRepository(DataContext dataContext) : IUserRepository
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task AddAsync(UserModel user, CancellationToken cancellationToken) =>      
            await _dataContext.Admins.AddAsync(new Admin { Username = user.Username, Password = user.Username }, cancellationToken);
        
        public async Task<Admin?> GetExistAsync(string username, CancellationToken cancellationToken) =>     
            await _dataContext.Admins.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        
        public async Task<bool> LoginAsync(string username, string passsword, CancellationToken cancellationToken)
        {
            var existUser = await _dataContext.Admins.FirstOrDefaultAsync(u => u.Username == username && u.Password == passsword, cancellationToken);
            if (existUser != null)
                return true;

            return false;
        }
    }
}
