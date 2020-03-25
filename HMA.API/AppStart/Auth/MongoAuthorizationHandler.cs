using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Services.Interfaces;
using HMA.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace HMA.API.AppStart.Auth
{
    public class MongoAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IUserT1Service _userService;

        public MongoAuthorizationHandler(IUserT1Service userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingClaimsAuthorizationRequirement = context.PendingRequirements
                .Where(pr => pr is ClaimsAuthorizationRequirement)
                .Cast<ClaimsAuthorizationRequirement>()
                .Where(pcar => pcar.ClaimType.Equals(ClaimsConstants.Registered, StringComparison.Ordinal))
                .ToList();

            if (!pendingClaimsAuthorizationRequirement.Any())
            {
                return;
            }

            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity?.FindFirst(ClaimsConstants.NameIdentifier);
            if (nameIdentifierClaim == null)
            {
                return;
            }

            if (!decimal.TryParse(nameIdentifierClaim.Value, out decimal userId))
            {
                return;
            }

            var isUserExists = await _userService.ExistsAsync(userId);
            if (isUserExists)
            {
                var userRegisteredClaim = new Claim(ClaimsConstants.Registered, true.ToString(), ClaimValueTypes.Boolean);
                claimsIdentity.AddClaim(userRegisteredClaim);

                pendingClaimsAuthorizationRequirement
                    .ForEach(pcar => context.Succeed(pcar));
            }
        }
    }
}
