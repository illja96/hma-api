using System;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HMA.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class SwaggerController : ControllerBase
    {
        private readonly IOptions<GoogleOptions> _googleOptions;

        public SwaggerController(IOptions<GoogleOptions> googleOptions)
        {
            _googleOptions = googleOptions;
        }

        /// <summary>
        /// Rewrites query parameters to be compatible with OAuth2 + OIDC.
        /// Set response_type to id_token.
        /// Add nonce.
        /// Redirects host to Google authorization endpoint
        /// </summary>
        /// <returns></returns>

        [HttpGet("swagger/fake-oauth")]
        [ProducesResponseType(302)]
        public IActionResult FakeOAuth()
        {
            var queryString = Request.QueryString.ToString();
            queryString += $"&nonce={Guid.NewGuid()}";
            queryString = queryString.Replace("response_type=token", "response_type=id_token");

            var url = $"{_googleOptions.Value.AuthorizationEndpoint}{queryString}";
            return Redirect(url);
        }
    }
}