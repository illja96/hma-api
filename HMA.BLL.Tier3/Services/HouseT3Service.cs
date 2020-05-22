using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.BLL.Tier3.Services.Interfaces;
using HMA.DTO.Models.House;
using HMA.DTO.Models.User;
using HMA.DTO.ViewModels.House;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;

namespace HMA.BLL.Tier3.Services
{
    public class HouseT3Service : IHouseT3Service
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IHouseT2Service _houseService;

        public HouseT3Service(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IHouseT2Service houseService)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _houseService = houseService;
        }

        public async Task<ObjectResult> GetHousesAsync(CancellationToken cancellationToken = default)
        {
            var userFromIdentity = GetUserFromIdentity();

            var availableHousesInfo = await _houseService.GetAvailableHouseInfosForUserAsync(userFromIdentity.GoogleId, cancellationToken);
            var availableHousesInfoViewModel = _mapper.Map<AvailableHousesInfoViewModel>(availableHousesInfo);

            var result = new OkObjectResult(availableHousesInfoViewModel);
            return result;
        }

        public async Task<ObjectResult> GetHouseByIdAsync(
            string houseId,
            CancellationToken cancellationToken = default)
        {
            BsonObjectId bsonHouseId;
            try
            {
                bsonHouseId = _mapper.Map<BsonObjectId>(houseId);
            }
            catch (AutoMapperMappingException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(houseId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var userFromIdentity = GetUserFromIdentity();

            try
            {
                var houseSimpleInfo = await _houseService.GetHouseSimpleInfoByIdAvailableForUserAsync(
                    bsonHouseId,
                    userFromIdentity.GoogleId,
                    cancellationToken);
                var houseSimpleInfoViewModel = _mapper.Map<HouseSimpleInfoViewModel>(houseSimpleInfo);

                var result = new OkObjectResult(houseSimpleInfoViewModel);
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
            var userFromIdentity = GetUserFromIdentity();

            var houseCreationRequest = _mapper.Map<HouseCreationRequest>(houseCreationRequestViewModel);
            houseCreationRequest.OwnerId = userFromIdentity.GoogleId;

            var houseSimpleInfo = await _houseService.CreateHouseAsync(houseCreationRequest, cancellationToken);

            var houseInfoViewModel = _mapper.Map<HouseSimpleInfoViewModel>(houseSimpleInfo);

            var result = new OkObjectResult(houseInfoViewModel);
            return result;
        }

        public async Task<ObjectResult> DeleteOrLeaveHouseByIdAsync(
            string houseId,
            CancellationToken cancellationToken = default)
        {
            BsonObjectId bsonHouseId;
            try
            {
                bsonHouseId = _mapper.Map<BsonObjectId>(houseId);
            }
            catch (AutoMapperMappingException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(houseId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var userFromIdentity = GetUserFromIdentity();

            try
            {
                await _houseService.DeleteHouseByIdOwnedByUserAsync(
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

            // TODO: Implement house leave for member
        }

        private UserInfo GetUserFromIdentity()
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            return userFromIdentity;
        }
    }
}
