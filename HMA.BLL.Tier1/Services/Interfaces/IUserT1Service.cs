using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.User;
using HMA.DTO.Models.User;

namespace HMA.BLL.Tier1.Services.Interfaces
{
    /// <summary>
    /// User service
    /// </summary>
    public interface IUserT1Service
    {
        /// <summary>
        /// Check if user exists
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<bool> ExistsAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="UserNotFoundException"></exception>
        Task<UserInfo> GetByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="UserNotFoundException"></exception>
        Task<UserInfo> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get users by ids
        /// </summary>
        /// <param name="userIds">User ids</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<List<UserSimpleInfo>> GetByIdsAsync(
            List<decimal> userIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="userInfo">User</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="UserDuplicateInsertionException"></exception>
        /// <exception cref="UserEmailNotVerifiedException"></exception>
        Task<UserInfo> RegisterAsync(
            UserInfo userInfo,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Update user info
        /// </summary>
        /// <param name="userInfo">User</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="UserEmailNotVerifiedException"></exception>
        Task<UserInfo> UpdateAsync(
            UserInfo userInfo,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="UserNotFoundException"></exception>
        Task DeleteByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default);
    }
}
