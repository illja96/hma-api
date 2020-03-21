using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<UserInfo> GetAsync(
            string userGoogleId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userGoogleId);

            var user = await _userInfoRepository.FindOneAsync(filter, cancellationToken);
            return user;
        }

        public async Task<List<UserInfo>> GetAsync(
            List<string> userGoogleIds,
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
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, user.GoogleId);
            var usersWithSameGoogleIdCount = await _userInfoRepository.CountAsync(filter, cancellationToken);
            var isUserAlreadyExists = usersWithSameGoogleIdCount != 0;
            if (isUserAlreadyExists)
            {
                throw new ArgumentException("User with same GoogleId already exists", nameof(user.GoogleId));
            }

            user = await _userInfoRepository.InsertAsync(user, cancellationToken);

            return user;
        }

        public async Task<UserInfo> UpdateAsync(
            UserInfo user,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, user.GoogleId);
            var update = Builders<UserInfo>.Update
                .Set(ui => ui.Email, user.Email)
                .Set(ui => ui.EmailVerified, user.EmailVerified)
                .Set(ui => ui.Picture, user.Picture)
                .Set(ui => ui.Locale, user.Locale)
                .Set(ui => ui.GivenName, user.GivenName)
                .Set(ui => ui.FamilyName, user.FamilyName);

            user = await _userInfoRepository.UpdateAsync(filter, update, cancellationToken);

            return user;
        }

        public async Task<long> DeleteAsync(
            string userGoogleId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userGoogleId);
            var deletedUsers = await _userInfoRepository.DeleteAsync(filter, cancellationToken);

            return deletedUsers;
        }
    }
}
