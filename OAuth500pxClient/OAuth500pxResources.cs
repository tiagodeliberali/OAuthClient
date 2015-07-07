using OAuth.Client;

namespace OAuth.Client500px
{
    public class OAuth500pxResources : IOAuth500pxResources
    {
        // interface IOAuthResources
        public string ConsumerKey { get { return "CONSUMER_KEY_FROM_YOUR_500PX_DEV_ACCOUNT"; } }
        public string ConsumerSecret { get { return "CONSUMER_SECRET_FROM_YOUR_500PX_DEV_ACCOUNT"; } }
        public string AccessTokenURL { get { return "https://api.500px.com/v1/oauth/access_token"; } }
        public string AuthorizeURL { get { return "https://api.500px.com/v1/oauth/authorize"; } }
        public string RequestTokenURL { get { return "https://api.500px.com/v1/oauth/request_token"; } }
        public string CallbackURL { get { return "https://notification500px.azure-mobile.net/callback"; } }

        // interface IOAuth500pxResources
        public int MaxPhotosPerPage { get { return 100; } }
        public string UserUrl { get { return "https://api.500px.com/v1/users"; } }
        public string PhotosUrl { get { return "https://api.500px.com/v1/photos"; } }
    }
}