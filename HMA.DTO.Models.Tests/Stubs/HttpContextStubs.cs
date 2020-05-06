using System.Collections.Generic;
using System.Security.Claims;
using HMA.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;

namespace HMA.DTO.Models.Tests.Stubs
{
    public static class HttpContextStubs
    {
        public static HttpContext HttpContextWithUser;

        static HttpContextStubs()
        {
            HttpContextWithUser = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new List<Claim>()
                            {
                                new Claim(ClaimsConstants.NameIdentifier, "123456789"),
                                new Claim(ClaimsConstants.EmailAddress, "test@ivaxor.com"),
                                new Claim(ClaimsConstants.EmailVerified, "true"),
                                new Claim(ClaimsConstants.Picture, "http://ivaxor.com/logo.png"),
                                new Claim(ClaimsConstants.GivenName, "Test"),
                                new Claim(ClaimsConstants.SurName, "Test"),
                                new Claim(ClaimsConstants.Locale, "en-us")
                            })))
            };
        }
    }
}
