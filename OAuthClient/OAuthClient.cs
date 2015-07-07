using System.Collections.Generic;
using System.Threading.Tasks;

namespace OAuth.Client
{
    public class OAuthClient : IOAuthClient
    {
        private IOAuthResources Resources { get; set; }
        private IOauthTools Tools { get; set; }


        public OAuthClient(IOAuthResources resources, IOauthTools tools)
        {
            Resources = resources;
            Tools = tools;
        }

        public async Task<RequestTokenInfo> GetRequestToken()
        {
            OAuthRequest client = OAuthRequest.ForRequestToken(
                Resources.ConsumerKey,
                Resources.ConsumerSecret,
                Resources.CallbackURL);

            client.RequestUrl = Resources.RequestTokenURL;

            // Using URL query authorization
            var responseString = await Tools.GetStringResponse(string.Format("{0}?{1}", client.RequestUrl, client.GetAuthorizationQuery()));

            var tokenValues = GetTokenValues(responseString);

            var requestTokenInfo = new RequestTokenInfo();

            requestTokenInfo.RequestToken = tokenValues[0];
            requestTokenInfo.RequestSecret = tokenValues[1];
            requestTokenInfo.CallbackConfirmed = tokenValues[2];
            requestTokenInfo.CallbackUrl = Resources.CallbackURL;

            requestTokenInfo.AccessUrl = string.Format("{0}?oauth_token={1}", Resources.AuthorizeURL, requestTokenInfo.RequestToken);

            return requestTokenInfo;
        }

        public async Task<AccessTokenInfo> GetAccessToken(RequestTokenInfo requestTokenInfo)
        {
            var client = OAuthRequest.ForAccessToken(
                Resources.ConsumerKey,
                Resources.ConsumerSecret,
                requestTokenInfo.RequestToken,
                requestTokenInfo.RequestSecret,
                requestTokenInfo.Verifier);

            client.RequestUrl = Resources.AccessTokenURL;

            var accessTokenString = await Tools.GetStringResponse(client.RequestUrl + "?" + client.GetAuthorizationQuery());

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