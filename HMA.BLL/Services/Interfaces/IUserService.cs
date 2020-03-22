using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.DTO.Models;

namespace HMA.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> Exists(
            decimal userGoogleId,
            CancellationToken cancellationToken = default);

        Task<UserInfo> GetAsync(
            decimal userGoogleId,
            CancellationToken cancellationToken = default);

        Task<List<UserInfo>> GetAsync(
            List<decimal> userGoogleIds,
            CancellationToken cancellationToken = default);

        Task<UserInfo> RegisterAsync(
            UserInfo user,
            CancellationToken cancellationToken = default);

        Task<UserInfo> UpdateAsync(
            UserInfo user,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            decimal userGoogleId,
            CancellationToken cancellationToken = default);
    }
}
