namespace OAuthClient
{
    public class RequestTokenInfo
    {
        public string RequestToken { get; set; }
        public string RequestSecret { get; set; }
        public string Verifier { get; set; }
        public string CallbackConfirmed { get; set; }
        public string CallbackUrl { get; set; }
        public string AccessUrl { get; set; }
    }
}