﻿using WebSpectre.Server.Data;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task AddAsync(UserModel user, CancellationToken cancellationToken);

        public Task<Admin?> GetExistAsync(string username, CancellationToken cancellationToken);

        public Task<bool> LoginAsync(string username, string passsword, CancellationToken cancellationToken);
    }
}
