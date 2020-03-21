using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.DTO.Models;

namespace HMA.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserInfo> GetAsync(
            string userGoogleId,
            CancellationToken cancellationToken = default);

        Task<List<UserInfo>> GetAsync(
            List<string> userGoogleIds,
            CancellationToken cancellationToken = default);

        Task<UserInfo> RegisterAsync(
            UserInfo user,
            CancellationToken cancellationToken = default);

        Task<UserInfo> UpdateAsync(
            UserInfo user,
            CancellationToken cancellationToken = default);

        Task<long> DeleteAsync(
            string userGoogleId,
            CancellationToken cancellationToken = default);
    }
}
