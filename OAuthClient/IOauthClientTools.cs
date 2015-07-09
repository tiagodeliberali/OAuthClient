using System.Threading.Tasks;

namespace OAuth.Client
{
    public interface IOauthClientTools
    {
        Task<string> GetStringResponse(string url);
    }
}
