using System.Threading.Tasks;

namespace OAuth.Client
{
    public interface IOAuthClient
    {
        Task<RequestTokenInfo> GetRequestToken();
        Task<AccessTokenInfo> GetAccessToken(RequestTokenInfo requestTokenInfo);
    }
}
