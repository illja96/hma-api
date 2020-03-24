﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.DTO.Models;

namespace HMA.BLL.Services.Interfaces
{
    /// <summary>
    /// User service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Check if user exists
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserInfo> GetByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get users by ids
        /// </summary>
        /// <param name="userIds">User ids</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserInfo>> GetByIdsAsync(
            List<decimal> userIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserInfo> RegisterAsync(
            UserInfo user,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Update user info
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserInfo> UpdateAsync(
            UserInfo user,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default);
    }
}
