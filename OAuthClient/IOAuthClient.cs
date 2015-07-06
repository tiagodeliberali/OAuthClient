using System.Threading.Tasks;

namespace OAuthClient
{
    public interface IOAuthClient
    {
        Task<string> GetStringResponse(string url);
        Task<RequestTokenInfo> GetRequestTokenInfo();
        Task<AccessTokenInfo> GetAccessToken(RequestTokenInfo requestTokenInfo);
    }
}
