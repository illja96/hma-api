using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier3.Services.Interfaces;
using HMA.DTO.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMA.API.Controllers
{
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserT3Service _userService;

        public UserController(IUserT3Service userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("users/register")]
        [ProducesResponseType(typeof(UserInfoViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> RegisterAsync(CancellationToken cancellationToken = default)
        {
            var result = await _userService.RegisterAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("users/me")]
        [ProducesResponseType(typeof(UserInfoViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            var result = await _userService.GetCurrentAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Updates current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("users/me")]
        [ProducesResponseType(typeof(UserInfoViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCurrentAsync(CancellationToken cancellationToken = default)
        {
            var result = await _userService.UpdateCurrentAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Delete user profile
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("users/me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProfileAsync(CancellationToken cancellationToken = default)
        {
            var result = await _userService.DeleteProfileAsync(cancellationToken);
            return result;
        }
    }
}