using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Exceptions.House;
using HMA.BLL.Services.Interfaces;
using HMA.BLL.Wrappers.Wrappers.Interfaces;
using HMA.DTO.Models;
using HMA.DTO.Models.House;
using HMA.DTO.ViewModels.House;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;

namespace HMA.BLL.Wrappers.Wrappers
{
    public class WrappedHouseService : IWrappedHouseService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IHouseService _houseService;

        public WrappedHouseService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IHouseService houseService)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _houseService = houseService;
        }

        public async Task<ObjectResult> GetHousesAsync(CancellationToken cancellationToken = default)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            var ownedHouseInfos = await _houseService.GetOwnedHousesAsync(
                userFromIdentity.GoogleId,
                cancellationToken);

            var memberOfHouseInfos = await _houseService.GetMemberOfHousesAsync(
                userFromIdentity.GoogleId,
                cancellationToken);

            var ownedHouseInfoViewModels = _mapper.Map<List<HouseSimpleInfoViewModel>>(ownedHouseInfos);
            var memberOfHouseInfoViewModels = _mapper.Map<List<HouseSimpleInfoViewModel>>(memberOfHouseInfos);

            var houseInfosViewModel = new HouseInfosViewModel()
            {
                Owned = ownedHouseInfoViewModels,
                MemberOf = memberOfHouseInfoViewModels
            };

            var result = new OkObjectResult(houseInfosViewModel);
            return result;
        }

        public async Task<ObjectResult> GetHouseByIdAsync(
            string houseId,
            CancellationToken cancellationToken = default)
        {
            BsonObjectId bsonHouseId;
            try
            {
                bsonHouseId = new BsonObjectId(new ObjectId(houseId));
            }
            catch (IndexOutOfRangeException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(houseId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            try
            {
                var houseInfo = await _houseService.GetHouseByIdAsync(
                    bsonHouseId,
                    userFromIdentity.GoogleId,
                    cancellationToken);

                var houseInfoViewModel = _mapper.Map<HouseInfoViewModel>(houseInfo);

                var result = new OkObjectResult(houseInfoViewModel);
                return result;
            }
            catch (HouseNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }
        }

        public async Task<ObjectResult> CreateHouseAsync(
            HouseCreationRequestViewModel houseCreationRequestViewModel,
            CancellationToken cancellationToken = default)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            var houseCreationRequest = _mapper.Map<HouseCreationRequest>(houseCreationRequestViewModel);
            houseCreationRequest.OwnerId = userFromIdentity.GoogleId;

            var houseInfo = await _houseService.CreateHouseAsync(houseCreationRequest, cancellationToken);

            var houseInfoViewModel = _mapper.Map<HouseInfoViewModel>(houseInfo);

            var result = new OkObjectResult(houseInfoViewModel);
            return result;
        }

        public async Task<ObjectResult> DeleteHouseByIdAsync(
            string houseId,
            CancellationToken cancellationToken = default)
        {
            BsonObjectId bsonHouseId;
            try
            {
                bsonHouseId = new BsonObjectId(new ObjectId(houseId));
            }
            catch (IndexOutOfRangeException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(houseId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            try
            {
                await _houseService.DeleteHouseByIdAsync(
                    bsonHouseId,
                    userFromIdentity.GoogleId,
                    cancellationToken);

                var result = new OkObjectResult(null);
                return result;
            }
            catch (HouseNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }
        }
    }
}
