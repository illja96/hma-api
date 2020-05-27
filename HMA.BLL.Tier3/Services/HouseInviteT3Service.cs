using AutoMapper;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.BLL.Tier3.Services.Interfaces;
using HMA.DTO.Models.Invite;
using HMA.DTO.Models.User;
using HMA.DTO.ViewModels.Invite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier1.Exceptions.Invite;

namespace HMA.BLL.Tier3.Services
{
    public class HouseInviteT3Service : IHouseInviteT3Service
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IHouseInviteT2Service _houseInviteService;

        public HouseInviteT3Service(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IHouseInviteT2Service houseInviteService)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

            _houseInviteService = houseInviteService;
        }

        public async Task<ObjectResult> GetInvitesAsync(CancellationToken cancellationToken = default)
        {
            var userFromIdentity = GetUserFromIdentity();

            var invites = await _houseInviteService.GetInvitesAvailableForUserAsync(
                userFromIdentity.GoogleId,
                cancellationToken);

            var inviteViewModels = _mapper.Map<List<HouseInviteSimpleInfoViewModel>>(invites);

            var result = new OkObjectResult(inviteViewModels);
            return result;
        }

        public async Task<ObjectResult> CreateInviteAsync(
            HouseInviteCreationRequestViewModel houseInviteCreationRequestViewModel,
            CancellationToken cancellationToken = default)
        {
            var userFromIdentity = GetUserFromIdentity();

            var houseInviteCreationRequest = _mapper.Map<HouseInviteCreationRequest>(houseInviteCreationRequestViewModel);
            houseInviteCreationRequest.InvitedByUserId = userFromIdentity.GoogleId;

            HouseInviteSimpleInfo houseInvite;
            try
            {
                houseInvite = await _houseInviteService.CreateInviteAsync(
                    houseInviteCreationRequest,
                    cancellationToken);
            }
            catch (TooManyHouseInvitesException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(
                    nameof(houseInviteCreationRequestViewModel.HouseId),
                    "Too many requests from you are still pending acceptance");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }
            catch (HouseInviteDuplicateInsertionException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(
                    nameof(houseInviteCreationRequestViewModel.UserEmail),
                    "User already invited");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }
            catch (HouseNotFoundException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(
                    nameof(houseInviteCreationRequestViewModel.HouseId),
                    "House not found");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }
            catch (SelfReferencingHouseInviteException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(
                    nameof(houseInviteCreationRequestViewModel.UserEmail),
                    "House owner can't invite himself");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var houseInviteViewModel = _mapper.Map<HouseInviteSimpleInfoViewModel>(houseInvite);

            var result = new OkObjectResult(houseInviteViewModel);
            return result;
        }

        public async Task<ObjectResult> AcceptInviteAsync(
            string inviteId,
            CancellationToken cancellationToken = default)
        {
            BsonObjectId bsonInviteId;
            try
            {
                bsonInviteId = _mapper.Map<BsonObjectId>(inviteId);
            }
            catch (AutoMapperMappingException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(inviteId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var userFromIdentity = GetUserFromIdentity();

            try
            {
                await _houseInviteService.AcceptInviteAvailableForUserAsync(
                    userFromIdentity.GoogleId,
                    bsonInviteId,
                    cancellationToken);
            }
            catch (HouseInviteNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }

            var result = new OkObjectResult(string.Empty);
            return result;
        }

        public async Task<ObjectResult> DeclineInviteAsync(
            string inviteId,
            CancellationToken cancellationToken = default)
        {
            BsonObjectId bsonInviteId;
            try
            {
                bsonInviteId = _mapper.Map<BsonObjectId>(inviteId);
            }
            catch (AutoMapperMappingException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(inviteId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var userFromIdentity = GetUserFromIdentity();

            try
            {
                await _houseInviteService.DeclineInviteAvailableForUserAsync(
                    userFromIdentity.GoogleId,
                    bsonInviteId,
                    cancellationToken);
            }
            catch (HouseInviteNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }

            var result = new OkObjectResult(string.Empty);
            return result;
        }

        private UserInfo GetUserFromIdentity()
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            return userFromIdentity;
        }
    }
}
