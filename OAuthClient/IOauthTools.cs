using System.Threading.Tasks;

namespace OAuth.Client
{
    public interface IOauthTools
    {
        Task<string> GetStringResponse(string url);
    }
}
