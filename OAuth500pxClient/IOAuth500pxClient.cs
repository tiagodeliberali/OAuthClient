using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OAuth.Client500px
{
    public interface IOAuth500pxClient
    {
        Task<JObject> GetUser();
        Task<JObject> GetPhotos(string username, int page, int requestPerPage = 100);
    }
}