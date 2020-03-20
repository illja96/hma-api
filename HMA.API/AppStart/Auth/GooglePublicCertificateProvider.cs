using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace HMA.API.AppStart.Auth
{
    internal class GooglePublicCertificateProvider
    {
        private const string GoogleApiOAuth2Certs = "https://www.googleapis.com/oauth2/v1/certs";
        
        public async Task<List<X509SecurityKey>> GetAsync()
        {
            using var httpClient = new HttpClient();
            var rawResponse = await httpClient.GetStringAsync(GoogleApiOAuth2Certs);

            var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawResponse);

            var rawCertificates = response.Values.ToList();
            var certificates = rawCertificates
                .Select(rawCert => System.Text.Encoding.ASCII.GetBytes(rawCert))
                .Select(certBytes => new X509Certificate2(certBytes))
                .Select(cert => new X509SecurityKey(cert))
                .ToList();

            return certificates;
        }
    }
}
