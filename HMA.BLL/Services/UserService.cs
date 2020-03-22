using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Exceptions;
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

        public async Task<bool> Exists(
            decimal userGoogleId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userGoogleId);
            var usersCount = await _userInfoRepository.CountAsync(filter, cancellationToken);
            var isUserExists = usersCount != 0;

            return isUserExists;
        }

        public async Task<UserInfo> GetAsync(
            decimal userGoogleId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userGoogleId);

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

        public async Task<List<UserInfo>> GetAsync(
            List<decimal> userGoogleIds,
            CancellationToken cancellationToken = default)
        {
            var filters = userGoogleIds
                .Select(userGoogleId => Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userGoogleId))
                .ToList();

            var filter = Builders<UserInfo>.Filter.Or(filters);

            var users = await _userInfoRepository.FindAsync(filter, cancellationToken);
            return users;
        }

        public async Task<UserInfo> RegisterAsync(
            UserInfo user,
            CancellationToken cancellationToken = default)
        {
            ThrowExceptionIfUserEmailIsNotVerified(user);

            var isUserExists = await Exists(user.GoogleId, cancellationToken);
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

            var isUserExists = await Exists(user.GoogleId, cancellationToken);
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

        public async Task DeleteAsync(
            decimal userGoogleId,
            CancellationToken cancellationToken = default)
        {
            var isUserExists = await Exists(userGoogleId, cancellationToken);
            if (!isUserExists)
            {
                throw new UserNotFoundException();
            }

            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userGoogleId);
            await _userInfoRepository.DeleteAsync(filter, cancellationToken);
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
