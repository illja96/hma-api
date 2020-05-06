using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.User;
using HMA.DTO.Models.User;

namespace HMA.BLL.Tier2.Services.Interfaces
{
    /// <summary>
    /// User service
    /// </summary>
    public interface IUserT2Service
    {
        /// <summary>
        /// Check if user exists
        /// </summary>
        /// <param name="userInfo">User info</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="UserDuplicateInsertionException"></exception>
        /// <exception cref="UserEmailNotVerifiedException"></exception>
        Task<UserInfo> RegisterAsync(
            UserInfo userInfo,
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
        /// Update user info
        /// </summary>
        /// <param name="userInfo">User info</param>
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
