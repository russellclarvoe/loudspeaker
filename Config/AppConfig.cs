using System;
using Microsoft.Extensions.Configuration;

namespace Loudspeaker.Config;

public class AppConfig
{
    public string FirebaseApiKey { get; }
    public string FirebaseAuthDomain { get; }
    public string FirebaseProjectId { get; }
    public string? GoogleOAuthClientId { get; }
    public string? GoogleOAuthClientSecret { get; }
    public string ApiBaseUrl { get; }
    public string TokenExchangeEndpoint { get; }

    public AppConfig(IConfiguration configuration)
    {
        FirebaseApiKey = configuration["Firebase:ApiKey"] 
            ?? throw new InvalidOperationException("Firebase:ApiKey is required in appsettings.json");
        FirebaseAuthDomain = configuration["Firebase:AuthDomain"] 
            ?? throw new InvalidOperationException("Firebase:AuthDomain is required in appsettings.json");
        FirebaseProjectId = configuration["Firebase:ProjectId"] 
            ?? throw new InvalidOperationException("Firebase:ProjectId is required in appsettings.json");
        GoogleOAuthClientId = configuration["Firebase:GoogleOAuthClientId"];
        GoogleOAuthClientSecret = configuration["Firebase:GoogleOAuthClientSecret"]; // optional
        ApiBaseUrl = configuration["Api:BaseUrl"] 
            ?? throw new InvalidOperationException("Api:BaseUrl is required in appsettings.json");
        TokenExchangeEndpoint = configuration["Api:TokenExchangeEndpoint"] 
            ?? throw new InvalidOperationException("Api:TokenExchangeEndpoint is required in appsettings.json");
    }
}

