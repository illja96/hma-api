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

            var invites = await _houseInviteService.GetInvitesAsync(userFromIdentity.GoogleId, cancellationToken);
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

            var houseInvite = await _houseInviteService.CreateInviteAsync(
                houseInviteCreationRequest,
                cancellationToken);

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

            await _houseInviteService.AcceptInviteAsync(
                userFromIdentity.GoogleId,
                bsonInviteId,
                cancellationToken);

            var result = new OkObjectResult(null);
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

            await _houseInviteService.DeclineInviteAsync(
                userFromIdentity.GoogleId,
                bsonInviteId,
                cancellationToken);

            var result = new OkObjectResult(null);
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
