﻿using System.Net.Http;
using System.Threading.Tasks;

namespace OAuth.Client
{
    public class OauthClientTools : IOauthClientTools
    {
        public async Task<string> GetStringResponse(string url)
        {
            var client = new HttpClient();
            var responseString = await client.GetStringAsync(url);

            return responseString;
        }
    }
}
