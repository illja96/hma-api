using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Services.Interfaces;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.DTO.Models.User;

namespace HMA.BLL.Tier2.Services
{
    public class UserT2Service : IUserT2Service
    {
        private readonly IUserT1Service _userService;

        public UserT2Service(IUserT1Service userService)
        {
            _userService = userService;
        }

        public async Task<UserInfo> RegisterAsync(
            UserInfo userInfo,
            CancellationToken cancellationToken = default)
        {
            userInfo = await _userService.RegisterAsync(userInfo, cancellationToken);
            return userInfo;
        }

        public async Task<UserInfo> GetByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var userInfo = await _userService.GetByIdAsync(userId, cancellationToken);
            return userInfo;
        }

        public async Task<UserInfo> UpdateAsync(
            UserInfo userInfo,
            CancellationToken cancellationToken = default)
        {
            userInfo = await _userService.UpdateAsync(userInfo, cancellationToken);
            return userInfo;
        }

        public async Task DeleteByIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            await _userService.DeleteByIdAsync(userId, cancellationToken);
        }
    }
}
