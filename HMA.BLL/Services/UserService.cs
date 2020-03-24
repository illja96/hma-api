using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Exceptions.User;
using HMA.BLL.Services.Interfaces;
using HMA.DAL.Repositories.Interfaces;
using HMA.DTO.Models;
using MongoDB.Driver;

namespace HMA.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<UserInfo> _userInfoRepository;

        public UserService(IGenericRepository<UserInfo> userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public async Task<bool> ExistsAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userId);
            var usersCount = await _userInfoRepository.CountAsync(filter, cancellationToken);
            var isUserExists = usersCount != 0;

            return isUserExists;
        }

        public async Task<UserInfo> GetByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userId);

            try
            {
                var user = await _userInfoRepository.FindOneAsync(filter, cancellationToken);
                return user;
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Equals("Sequence contains no elements", StringComparison.Ordinal))
                {
                    throw new UserNotFoundException();
                }

                throw;
            }
        }

        public async Task<List<UserInfo>> GetByIdsAsync(
            List<decimal> userIds,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.In(ui => ui.GoogleId, userIds);

            var users = await _userInfoRepository.FindAsync(filter, cancellationToken);

            return users;
        }

        public async Task<UserInfo> RegisterAsync(
            UserInfo user,
            CancellationToken cancellationToken = default)
        {
            ThrowExceptionIfUserEmailIsNotVerified(user);

            var isUserExists = await ExistsAsync(user.GoogleId, cancellationToken);
            if (isUserExists)
            {
                throw new UserDuplicateInsertionException();
            }

            user.RegistrationDate = DateTime.UtcNow;
            user.LastUpdateDate = user.RegistrationDate;

            user = await _userInfoRepository.InsertAsync(user, cancellationToken);

            return user;
        }

        public async Task<UserInfo> UpdateAsync(
            UserInfo user,
            CancellationToken cancellationToken = default)
        {
            ThrowExceptionIfUserEmailIsNotVerified(user);

            var isUserExists = await ExistsAsync(user.GoogleId, cancellationToken);
            if (!isUserExists)
            {
                throw new UserNotFoundException();
            }

            user.LastUpdateDate = DateTime.UtcNow;

            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, user.GoogleId);
            var update = Builders<UserInfo>.Update
                .Set(ui => ui.Email, user.Email)
                .Set(ui => ui.EmailVerified, user.EmailVerified)
                .Set(ui => ui.Picture, user.Picture)
                .Set(ui => ui.Locale, user.Locale)
                .Set(ui => ui.GivenName, user.GivenName)
                .Set(ui => ui.FamilyName, user.FamilyName)
                .Set(ui => ui.LastUpdateDate, user.LastUpdateDate);

            await _userInfoRepository.UpdateAsync(filter, update, cancellationToken);

            user = await _userInfoRepository.FindOneAsync(filter, cancellationToken);

            return user;
        }

        public async Task<bool> DeleteByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userId);
            var deletedUsersCount = await _userInfoRepository.DeleteAsync(filter, cancellationToken);

            var isUserDeleted = deletedUsersCount == 1;

            return isUserDeleted;
        }

        private static void ThrowExceptionIfUserEmailIsNotVerified(UserInfo user)
        {
            if (!user.EmailVerified)
            {
                throw new UserEmailNotVerifiedException();
            }
        }
    }
}
