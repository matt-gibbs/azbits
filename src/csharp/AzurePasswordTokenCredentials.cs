using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace AzBits.Rest
{
    internal class AzurePasswordTokenCredentials : ServiceClientCredentials
    {
        private const string AzureAdAuthority = "https://login.windows.net/";
        private const string AzureRmResource = "https://management.azure.com/";
        private readonly string _applicationId;
        private readonly string _subscriptionId;
        private readonly string _password;
        private readonly string _tenantId;

        public AzurePasswordTokenCredentials(
            string tenantId,
            string subscriptionId,
            string applicationId,
            string password)
        {
            _tenantId = tenantId;
            _subscriptionId = subscriptionId;
            _applicationId = applicationId;
            _password = password;
            TokenExpires = new DateTimeOffset(DateTime.UtcNow);
        }

        public string Token { get; set; }

        public DateTimeOffset TokenExpires { get; set; }

        public async Task AcquireTokenAsync()
        {
            string authority = AzureAdAuthority + _tenantId;
            var authenticatonContext = new AuthenticationContext(authority,
                tokenCache: null,
                validateAuthority: true);

            var clientCredential = new ClientCredential(_applicationId, _password);

            AuthenticationResult result =
                await authenticatonContext.AcquireTokenAsync(AzureRmResource, clientCredential);

            Token = result.AccessToken;
            TokenExpires = result.ExpiresOn;
        }

        public void AcquireToken()
        {
            var task = AcquireTokenAsync();
            task.Wait();
        }

        public string SubscriptionId
        {
            get { return _subscriptionId; }
        }

        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if ((Token == null) || (TokenExpires < new DateTimeOffset(DateTime.UtcNow)))
            {
                AcquireToken();
            }
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
