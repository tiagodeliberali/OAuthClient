using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OAuth.Client;

namespace OAuth.Client500px
{
    public class OAuth500pxClient : IOAuth500pxClient
    {
        private IOAuth500pxResources Resources { get; set; }
        private AccessTokenInfo AccessToken { get; set; }
        private IOauthTools Tools { get; set; }

        public OAuth500pxClient(AccessTokenInfo accessTokenInfo, IOauthTools tools, IOAuth500pxResources resources)
        {
            AccessToken = accessTokenInfo;
            Tools = tools;
            Resources = resources;
        }

        public async Task<JObject> GetUser()
        {
            var client = OAuthRequest.ForProtectedResource(
                "GET", 
                Resources.ConsumerKey, 
                Resources.ConsumerSecret, 
                AccessToken.AccessToken, 
                AccessToken.AccessTokenSecret);

            client.RequestUrl = Resources.UserUrl;

            string url = string.Format("{0}?{1}", client.RequestUrl, client.GetAuthorizationQuery());

            return ToJson(await Tools.GetStringResponse(url));
        }

        public async Task<JObject> GetPhotos(string username, int page, int requestPerPage = 100)
        {
            string url = string.Format(
                "{0}?feature=user&image_size%5B%5D=200&sort=created_at&include_states=false&formats=jpeg%2Clytro&only_profile=1" +
                "&rpp={1}&page={2}&username={3}&consumer_key={4}",
                Resources.PhotosUrl,
                requestPerPage,
                page,
                username,
                Resources.ConsumerKey);

            return ToJson(await Tools.GetStringResponse(url));
        }

        private JObject ToJson(string value)
        {
            return JObject.Parse(value);
        }
    }
}