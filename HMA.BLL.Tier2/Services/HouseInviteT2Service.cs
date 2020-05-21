using HMA.BLL.Tier2.Services.Interfaces;
using HMA.DTO.Models.Invite;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.BLL.Tier2.Services
{
    public class HouseInviteT2Service : IHouseInviteT2Service
    {
        public Task<List<HouseInviteInfo>> GetInvitesAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<HouseInviteInfo> CreateInviteAsync(
            HouseInviteCreationRequest houseInviteCreationRequest,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AcceptInviteAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }        

        public Task DeclineInviteAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }        
    }
}
