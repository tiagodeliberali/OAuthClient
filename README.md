#OAuthClient

OAuthClient is a portable class library for .net that implements OAuth 1.0a, allowing access sites like 500px.com.

This project do some changes to [OAuth](https://github.com/danielcrenna/oauth), by Daniel Crenna, to turn it into a portable class library. 
This portable class library targets:

* .NET Framework 4.5
* Windows 8
* Windows Phone 8.1
* WIndows Phone Silverlight 8

In addition, it creates a client class with async methods and simple interface, to facilitate test and implementation of OAuth clients.

##Basic flow

Thinking about [how OAuth 1.0a works](http://www.cubrid.org/blog/dev-platform/dancing-with-oauth-understanding-how-authorization-works/), 
we can organise its flow in 4 stages:

* Gets a Request Token from Service Provider
* Redirects User to the Service Provider authentication page, passing the Request Token
* Gets the Verifier from the Service Provider, through the Callback Url query string parameters
* Takes all information gathered until now to request the Access Token

##Using OAuthClient

OAuthClient encapsulate this flow using async methods, very suitable for windows applications. 

Also, OAuthClient requires as a constructor parameter an instance of IOAuthResources, that must be implemented by you, with the values relevant to
your OAuth server.

Here is an example of implementation to access the 500px api:

```csharp
using OAuth.Client;

namespace OAuth500pxClient
{
    public class OAuth500pxResources : IOAuthResources
    {
        // interface IOAuthResources
        public string ConsumerKey { 
            get { return "CONSUMER_KEY_FROM_YOUR_500PX_DEV_ACCOUNT"; } 
        }
        public string ConsumerSecret { 
            get { return "CONSUMER_SECRET_FROM_YOUR_500PX_DEV_ACCOUNT"; } 
        }
        public string AccessTokenURL { 
            get { return "https://api.500px.com/v1/oauth/access_token"; } 
        }
        public string AuthorizeURL { 
            get { return "https://api.500px.com/v1/oauth/authorize"; } 
        }
        public string RequestTokenURL { 
            get { return "https://api.500px.com/v1/oauth/request_token"; } 
        }
        public string CallbackURL  { 
            get { return "https://notification500px.azure-mobile.net/callback"; } 
        }
    }
}
```

Now, let's see a simple example of an Windows Phone 8.1 application using our client.


###1. Get request token information and send the user to be authenticated by the Service Provider

```csharp
// SomePage.xaml.cs
using OAuth.Client;

...
    // button click event to login to the 500px api using OAuth
    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var client = new OAuthClient(new OAuth500pxResources());
    
        // RequestTokenInfo is kept as a static public property of App.xaml.cs
        App.RequestTokenInfo = await client.GetRequestTokenInfo();

        // Uses WebAuthenticationBroker to redirects the User to 
        // the Service Provider authentication page
        var requestUrl = new System.Uri(App.RequestTokenInfo.AccessUrl);
        var callbackUrl = new System.Uri(App.RequestTokenInfo.CallbackUrl);

        WebAuthenticationBroker.AuthenticateAndContinue(
            requestUrl, callbackUrl, null, WebAuthenticationOptions.None);
    }
```

###2. Recover Verifier and get the Access Token

After the user get authenticated to the OAuth server, WebAuthenticationBroker will get the callback url parameters,
including the verifier, and will pass it to the OnActivated method on App.xaml.cs.

```csharp
// App.xaml.cs
using OAuth.Client;

...
    // This property will be filled by LoginButton_Click on SomePage.xaml.cs
    public static RequestTokenInfo RequestTokenInfo { get; set; }
    
    // After user completes the authentication and authorize the Consumer, 
    // this method is called with the values passed to the callback url as a query 
    // string. It includes the verifier.
    protected async override void OnActivated(IActivatedEventArgs args)
    {
        if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation)
        {
            var webBrokerArg = args as WebAuthenticationBrokerContinuationEventArgs;

            // We will add this missing information to the RequestTokenInfo
            RequestTokenInfo.Verifier = GetVerifierFromParameters(
                webBrokerArg.WebAuthenticationResult.ResponseData);

            var client = new OAuthClient(new OAuth500pxResources());

            // Gets the Access Token from Service Provider
            var accessTokenInfo = await client.GetAccessToken(RequestTokenInfo);
        }

        base.OnActivated(args);
    }
```
