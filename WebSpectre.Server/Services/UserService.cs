using WebSpectre.Server.Models;
using WebSpectre.Server.Services.Interfaces;

namespace WebSpectre.Server.Services
{
    public class UserService : IUserService
    {
        public Task<UserModel> AddUserAsync(string username, string password, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> LoginUserAsync(string username, string password, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
