using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OAuth.Client;
using Xunit;

namespace OAuth.Client.Test
{
    public class OAuthClientTests
    {
        const string callbackUrl = "http://callback/url";
        const string authorizeUrl = "http://authorize/url";
        const string requestToken = "request_token";
        const string requestSecret = "request_secret";

        private RequestTokenInfo expectedRequestTokenInfo = new RequestTokenInfo()
        {
            AccessUrl = authorizeUrl + "?oauth_token=" + requestToken,
            CallbackConfirmed = "true",
            CallbackUrl = callbackUrl,
            RequestSecret = requestSecret,
            RequestToken = requestToken
        };

        [Fact]
        public async void GetValidRequestToken()
        {
            // Arrange
            var responseString = string.Format("oauth_token={0}&oauth_token_secret={1}&oauth_callback_confirmed=true", requestToken, requestSecret);

            var client = GivenAConfiguredClient(responseString);

            // Act
            var token = await client.GetRequestToken();

            // Assert
            ThenResultantTokenHasExpectedValues(token);
        }

        [Fact]
        public async void GetAccessToken()
        {

            // Arrange
            var responseString = string.Format("oauth_token={0}&oauth_token_secret={1}", requestToken, requestSecret);

            var client = GivenAConfiguredClient(responseString);

            // Act
            var token = await client.GetAccessToken(expectedRequestTokenInfo);

            // Assert
            Assert.Equal(requestToken, token.AccessToken);
            Assert.Equal(requestSecret, token.AccessTokenSecret);
        }

        private void ThenResultantTokenHasExpectedValues(RequestTokenInfo token)
        {
            Assert.Equal(expectedRequestTokenInfo.AccessUrl, token.AccessUrl);
            Assert.Equal(expectedRequestTokenInfo.CallbackConfirmed, token.CallbackConfirmed);
            Assert.Equal(expectedRequestTokenInfo.CallbackUrl, token.CallbackUrl);
            Assert.Equal(expectedRequestTokenInfo.RequestSecret, token.RequestSecret);
            Assert.Equal(expectedRequestTokenInfo.RequestToken, token.RequestToken);
        }

        private static OAuthClient GivenAConfiguredClient(string responseString)
        {
            var tools = new Mock<IOauthClientTools>();
            tools.Setup(t => t.GetStringResponse(It.IsAny<string>())).Returns(Task.FromResult(responseString));

            var resources = new Mock<IOAuthResources>();
            resources.Setup(r => r.ConsumerKey).Returns("consumer_key");
            resources.Setup(r => r.ConsumerSecret).Returns("consumer_secret");
            resources.Setup(r => r.CallbackURL).Returns(callbackUrl);
            resources.Setup(r => r.RequestTokenURL).Returns("http://request/token/url");
            resources.Setup(r => r.AuthorizeURL).Returns(authorizeUrl);
            resources.Setup(r => r.AccessTokenURL).Returns("http://access/token/url");

            var client = new OAuthClient(resources.Object, tools.Object);

            return client;
        }
    }
}
