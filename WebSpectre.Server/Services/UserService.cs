using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Server.Services.Interfaces;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddUserAsync(string username, string password, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetExistAsync(username, cancellationToken);
            if (existUser != null)
                throw new UserAlreadyExistException();

            await _userRepository.AddAsync(new UserModel
            {
                Username = username,
                Password = password,
            }, cancellationToken);
        }

        public async Task<UserModel?> LoginUserAsync(string username, string password, CancellationToken cancellationToken)
        {
            var loggedIn = await _userRepository.LoginAsync(username, password, cancellationToken);
            if (loggedIn)            
                return new UserModel { Username = username, Password = password };          
            
            return null;
        }
    }
}
