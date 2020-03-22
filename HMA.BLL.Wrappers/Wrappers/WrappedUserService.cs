using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Exceptions;
using HMA.BLL.Services.Interfaces;
using HMA.BLL.Wrappers.Wrappers.Interfaces;
using HMA.DTO.Models;
using HMA.DTO.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HMA.BLL.Wrappers.Wrappers
{
    public class WrappedUserService : IWrappedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        private readonly IUserService _userService;

        public WrappedUserService(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ObjectResult> RegisterAsync(CancellationToken cancellationToken = default)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            try
            {
                var user = await _userService.RegisterAsync(userFromIdentity, cancellationToken);
                var userViewModel = _mapper.Map<UserInfoViewModel>(user);

                var result = new OkObjectResult(userViewModel);
                return result;
            }
            catch (UserEmailNotVerifiedException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(UserInfo.EmailVerified), "Email is not verified");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }
            catch (UserDuplicateInsertionException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(UserInfo.GoogleId), "User already registered");

                var badResult = new ConflictObjectResult(modelState);
                return badResult;
            }
        }

        public async Task<ObjectResult> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            try
            {
                var user = await _userService.GetAsync(userFromIdentity.GoogleId, cancellationToken);
                var userViewModel = _mapper.Map<UserInfoViewModel>(user);

                var result = new OkObjectResult(userViewModel);
                return result;
            }
            catch (UserEmailNotVerifiedException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(UserInfo.EmailVerified), "Email is not verified");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }
            catch (UserNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }
        }

        public async Task<ObjectResult> UpdateCurrentAsync(CancellationToken cancellationToken = default)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            try
            {
                var user = await _userService.UpdateAsync(userFromIdentity, cancellationToken);
                var userViewModel = _mapper.Map<UserInfoViewModel>(user);

                var result = new OkObjectResult(userViewModel);
                return result;
            }
            catch (UserEmailNotVerifiedException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(UserInfo.EmailVerified), "Email is not verified");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }
            catch (UserNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }
        }

        public async Task<StatusCodeResult> DeleteProfileAsync(CancellationToken cancellationToken = default)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            try
            {
                await _userService.DeleteAsync(
                    userFromIdentity.GoogleId,
                    cancellationToken);

                var result = new OkResult();
                return result;
            }
            catch (UserNotFoundException)
            {
                var badResult = new NotFoundResult();
                return badResult;
            }
        }
    }
}
