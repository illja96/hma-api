using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.User;
using HMA.BLL.Tier1.Services.Interfaces;
using HMA.DAL.Repositories.Interfaces;
using HMA.DTO.Models.User;
using MongoDB.Driver;

namespace HMA.BLL.Tier1.Services
{
    public class UserT1Service : IUserT1Service
    {
        private readonly IGenericRepository<UserInfo> _userInfoRepository;

        public UserT1Service(IGenericRepository<UserInfo> userInfoRepository)
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

        public async Task<UserInfo> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.Email, email);

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

        public async Task<List<UserSimpleInfo>> GetByIdsAsync(
            List<decimal> userIds,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.In(ui => ui.GoogleId, userIds);
            var projection = Builders<UserInfo>.Projection.Expression(ui => new UserSimpleInfo()
            {
                GoogleId = ui.GoogleId,
                Email = ui.Email,
                FamilyName = ui.FamilyName,
                GivenName = ui.GivenName,
                Picture = ui.Picture
            });

            var pipeline = new EmptyPipelineDefinition<UserInfo>()
                .Match(filter)
                .Project(projection);

            var userSimpleInfos = await _userInfoRepository.FindAsync(pipeline, cancellationToken);

            return userSimpleInfos;
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

            await _userInfoRepository.UpdateOneAsync(filter, update, cancellationToken);

            user = await _userInfoRepository.FindOneAsync(filter, cancellationToken);

            return user;
        }

        public async Task DeleteByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserInfo>.Filter.Eq(ui => ui.GoogleId, userId);
            var deletedUsersCount = await _userInfoRepository.DeleteAsync(filter, cancellationToken);

            var isUserDeleted = deletedUsersCount == 1;
            if (!isUserDeleted)
            {
                throw new UserNotFoundException();
            }
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
