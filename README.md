Still in alpha... I will do some renames and file moves today...

#OAuthClient

OAuth client for .net as a Portable class.

This class implements a portable version of [OAuth](https://github.com/danielcrenna/oauth), by Daniel Crenna, encapsulated with async methods, 
to facilitate test and implementation of OAuth clients.

##How to Use

The usage of the cass is very straightforward. You must create an instance of OAuthServer, passing as parameter an instance of 
your implementation of IOAuthResources. This interface defines the expected values used by OAuthServer to communicate with the real OAuth server.

**IMPORTANT: You must create you own implementation of IOAuthResources, with the values of your OAuth server.**

OAuthServer class implements IOAuthServer interface, to facilitate tests and mocks.

###1. Get request token information

```csharp
using OAuthClient;

...

var oAuthServer = new OAuthServer(new OAuthResources());
var requestTokenInfo = await oAuthServer.GetRequestTokenInfo();
```

###2. Get the verifier

Send the user to RequestTokenInfo.AccessUrl and get the Verifier code from the OAuth server


```csharp
// SomePage.xaml.cs
using OAuthClient;

...

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        var oAuthServer = new OAuthServer(new OAuthResources());
    
        App.RequestTokenInfo = await oAuthServer.GetRequestTokenInfo();

        var requestUrl = new System.Uri(App.RequestTokenInfo.AccessUrl);
        var callbackUrl = new System.Uri(App.RequestTokenInfo.CallbackUrl);

        WebAuthenticationBroker.AuthenticateAndContinue(requestUrl, callbackUrl, null, WebAuthenticationOptions.None);
    }
```

###3. Get the access token

After the user get authenticated to the OAuth server, get the verifier token, set it to the previously created RequestTokenInfo instance, than pass 
it to the GetAccessToken method.

```csharp
// App.xaml.cs
using OAuthClient;

...

    public static RequestTokenInfo RequestTokenInfo { get; set; }
    
    protected async override void OnActivated(IActivatedEventArgs args)
    {
        if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation)
        {
            var webAuthenticationBrokerArg = args as WebAuthenticationBrokerContinuationEventArgs;

            RequestTokenInfo.Verifier = webAuthenticationBrokerArg.WebAuthenticationResult.ResponseData.Split('=')[2];

            var oAuthServer = new OAuthServer(new OAuthResources());

            var accessTokenInfo = await oAuthServer.GetAccessToken(RequestTokenInfo);
        }

        base.OnActivated(args);
    }
```
