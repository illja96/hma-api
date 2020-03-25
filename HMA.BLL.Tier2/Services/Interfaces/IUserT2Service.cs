using System.Threading;
using System.Threading.Tasks;
using HMA.DTO.Models.User;

namespace HMA.BLL.Tier2.Services.Interfaces
{
    public interface IUserT2Service
    {
        Task<UserInfo> RegisterAsync(
            UserInfo userInfo,
            CancellationToken cancellationToken = default);

        Task<UserInfo> GetByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        Task<UserInfo> UpdateAsync(
            UserInfo userInfo,
            CancellationToken cancellationToken = default);

        Task DeleteByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default);
    }
}
