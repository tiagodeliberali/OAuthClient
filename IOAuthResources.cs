namespace OAuthClient
{
    public interface IOAuthResources
    {
        string AccessTokenURL { get; }
        string AuthorizeURL { get; }
        string CallbackURL { get; }
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        string RequestTokenURL { get; }
    }
}
