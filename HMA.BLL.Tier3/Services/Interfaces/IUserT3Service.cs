using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.BLL.Tier3.Services.Interfaces
{
    /// <summary>
    /// User service
    /// </summary>
    public interface IUserT3Service
    {
        /// <summary>
        /// Register current user
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> RegisterAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get current user info
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> GetCurrentAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Update current user info
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<ObjectResult> UpdateCurrentAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete current user profile
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> DeleteProfileAsync(CancellationToken cancellationToken = default);
    }
}
