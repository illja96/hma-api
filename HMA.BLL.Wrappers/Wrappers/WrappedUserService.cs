using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
            catch (ArgumentException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(userFromIdentity.GoogleId), "User with same GoogleId already registered");

                var badResult = new ConflictObjectResult(modelState);
                return badResult;
            }
        }

        public async Task<ObjectResult> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            var user = await _userService.GetAsync(userFromIdentity.GoogleId, cancellationToken);
            var userViewModel = _mapper.Map<UserInfoViewModel>(user);

            var result = new OkObjectResult(userViewModel);
            return result;
        }

        public async Task<ObjectResult> DeleteProfileAsync(CancellationToken cancellation = default)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            var deletedUserCount = await _userService.DeleteAsync(userFromIdentity.GoogleId, cancellation);
            if (deletedUserCount != 1)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(userFromIdentity.GoogleId), "User with specified GoogleId is not registered");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var result = new OkObjectResult(null);
            return result;
        }
    }
}
