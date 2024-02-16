using WebSpectre.Server.Models;

namespace WebSpectre.Server.Services.Interfaces
{
    public interface IUserService
    {
        public Task<UserModel> AddUserAsync(string username, string password, CancellationToken cancellationToken);

        public Task<UserModel> LoginUserAsync(string username, string password, CancellationToken cancellationToken);
    }
}
