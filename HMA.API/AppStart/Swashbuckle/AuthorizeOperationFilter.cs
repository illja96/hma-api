using System.Collections.Generic;
using System.Linq;
using HMA.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HMA.API.AppStart.Swashbuckle
{
    internal class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Security.Clear();

            var isAllowAnonymous = IsAllowAnonymous(context);
            if (isAllowAnonymous)
            {
                return;
            }

            var policies = GetPolicies(context);
            if (policies.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse() { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse() { Description = "Forbidden" });
                
                operation.Security.Add(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "google-oauth2",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        ScopeConstants.Scopes
                    }
                });
            }
        }

        private bool IsAllowAnonymous(OperationFilterContext context)
        {
            var isAllowAnonymousFromController = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .OfType<AllowAnonymousAttribute>()
                .Any() ?? false;

            var isAllowAnonymousFromAction = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AllowAnonymousAttribute>()
                .Any();

            var isAllowAnonymous = isAllowAnonymousFromController || isAllowAnonymousFromAction;

            return isAllowAnonymous;
        }

        private List<string> GetPolicies(OperationFilterContext context)
        {
            var policiesFromController = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(a => a.Policy)
                .Distinct()
                .ToList() ?? new List<string>();

            var policiesFromAction = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(a => a.Policy)
                .Distinct()
                .ToList();

            var policies = new List<string>();
            policies.AddRange(policiesFromController);
            policies.AddRange(policiesFromAction);
            policies = policies
                .Distinct()
                .ToList();

            return policies;
        }

    }
}
