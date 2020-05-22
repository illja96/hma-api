using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Tier1.Exceptions.User;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.BLL.Tier3.Services.Interfaces;
using HMA.DTO.Models.User;
using HMA.DTO.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HMA.BLL.Tier3.Services
{
    public class UserT3Service : IUserT3Service
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUserT2Service _userService;

        public UserT3Service(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUserT2Service userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ObjectResult> RegisterAsync(CancellationToken cancellationToken = default)
        {
            var userFromIdentity = GetUserFromIdentity();

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
                modelState.AddModelError(nameof(UserInfo.IsEmailVerified), "Email is not verified");

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
            var userFromIdentity = GetUserFromIdentity();

            try
            {
                var user = await _userService.GetByIdAsync(userFromIdentity.GoogleId, cancellationToken);
                var userViewModel = _mapper.Map<UserInfoViewModel>(user);

                var result = new OkObjectResult(userViewModel);
                return result;
            }
            catch (UserEmailNotVerifiedException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(UserInfo.IsEmailVerified), "Email is not verified");

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
            var userFromIdentity = GetUserFromIdentity();

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
                modelState.AddModelError(nameof(UserInfo.IsEmailVerified), "Email is not verified");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }
            catch (UserNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }
        }

        public async Task<ObjectResult> DeleteProfileAsync(CancellationToken cancellationToken = default)
        {
            var userFromIdentity = GetUserFromIdentity();

            try
            {
                await _userService.DeleteByIdAsync(
                    userFromIdentity.GoogleId,
                    cancellationToken);

                var result = new OkObjectResult(null);
                return result;
            }
            catch (UserNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }
        }

        private UserInfo GetUserFromIdentity()
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            return userFromIdentity;
        }
    }
}
