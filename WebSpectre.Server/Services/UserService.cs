using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Server.Services.Interfaces;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task AddUserAsync(string username, string password, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetExistAsync(username, cancellationToken);
            if (existUser != null)
                throw new EntityAlreadyExistException();

            var hashedPass = HashCreator.CreateSHA256(password);

            await _userRepository.AddAsync(new UserModel
            {
                Username = username,
                Password = hashedPass,
            }, cancellationToken);
        }

        public async Task<UserModel?> LoginUserAsync(string username, string password, CancellationToken cancellationToken)
        {
            var hashedPass = HashCreator.CreateSHA256(password);

            var loggedIn = await _userRepository.LoginAsync(username, hashedPass, cancellationToken);
            if (loggedIn)            
                return new UserModel { Username = username, Password = password };          
            
            return null;
        }
    }
}
