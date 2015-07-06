using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using OAuth;

namespace OAuthClient
{
    public class OAuthClient : IOAuthClient
    {
        private IOAuthResources OAuthResources { get; set; }

        public OAuthClient(IOAuthResources resources)
        {
            OAuthResources = resources;
        }

        public async Task<string> GetStringResponse(string url)
        {
            var client = new HttpClient();
            var responseString = await client.GetStringAsync(url);

            return responseString;
        }

        public async Task<RequestTokenInfo> GetRequestTokenInfo()
        {
            OAuthRequest client = OAuthRequest.ForRequestToken(
                OAuthResources.ConsumerKey,
                OAuthResources.ConsumerSecret,
                OAuthResources.CallbackURL);

            client.RequestUrl = OAuthResources.RequestTokenURL;

            // Using URL query authorization
            var responseString = await GetStringResponse(string.Format("{0}?{1}", client.RequestUrl, client.GetAuthorizationQuery()));

            var tokenValues = GetTokenValues(responseString);

            var requestTokenInfo = new RequestTokenInfo();

            requestTokenInfo.RequestToken = tokenValues[0];
            requestTokenInfo.RequestSecret = tokenValues[1];
            requestTokenInfo.CallbackConfirmed = tokenValues[2];
            requestTokenInfo.CallbackUrl = OAuthResources.CallbackURL;

            requestTokenInfo.AccessUrl = string.Format("{0}?oauth_token={1}", OAuthResources.AuthorizeURL, requestTokenInfo.RequestToken);

            return requestTokenInfo;
        }

        public async Task<AccessTokenInfo> GetAccessToken(RequestTokenInfo requestTokenInfo)
        {
            var client = OAuthRequest.ForAccessToken(
                OAuthResources.ConsumerKey,
                OAuthResources.ConsumerSecret,
                requestTokenInfo.RequestToken,
                requestTokenInfo.RequestSecret,
                requestTokenInfo.Verifier);

            client.RequestUrl = OAuthResources.AccessTokenURL;

            var accessTokenString = await GetStringResponse(client.RequestUrl + "?" + client.GetAuthorizationQuery());

            var tokenValues = GetTokenValues(accessTokenString);

            var accessTokenInfo = new AccessTokenInfo();

            accessTokenInfo.AccessToken = tokenValues[0];
            accessTokenInfo.AccessTokenSecret = tokenValues[1];

            return accessTokenInfo;
        }

        private List<string> GetTokenValues(string token)
        {
            var values = new List<string>();

            var keyValues = token.Split('&');

            foreach (var item in keyValues)
            {
                values.Add(item.Split('=')[1]);
            }

            return values;
        }
    }
}