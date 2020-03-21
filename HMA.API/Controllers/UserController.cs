using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Wrappers.Wrappers.Interfaces;
using HMA.DTO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMA.API.Controllers
{
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IWrappedUserService _wrappedUserService;

        public UserController(IWrappedUserService wrappedUserService)
        {
            _wrappedUserService = wrappedUserService;
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("users/register")]
        [ProducesResponseType(typeof(UserInfoViewModel), 200)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> RegisterAsync(CancellationToken cancellationToken = default)
        {
            var result = await _wrappedUserService.RegisterAsync(cancellationToken);
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
            var result = await _wrappedUserService.GetCurrentAsync(cancellationToken);
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
            var result = await _wrappedUserService.DeleteProfileAsync(cancellationToken);
            return result;
        }
    }
}