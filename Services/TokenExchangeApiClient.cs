using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Loudspeaker.ApiClients;
using Loudspeaker.Config;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.Services;

public class TokenExchangeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly AppConfig _config;
    private readonly AuthControllerClient _authClient;

    public TokenExchangeApiClient(AppConfig config)
    {
        _config = config;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_config.ApiBaseUrl)
        };
        _authClient = new AuthControllerClient(_httpClient)
        {
            BaseUrl = _config.ApiBaseUrl
        };
    }

    public async Task<string> ExchangeIdTokenForCustomTokenAsync(string idToken, CancellationToken cancellationToken = default)
    {
        try
        {
            Log($"[TokenExchangeApiClient] Exchanging ID token for custom token using AuthenticateWithFirebaseAsync");
            
            var tokenRequest = new TokenRequest
            {
                IdToken = idToken
            };

            var response = await _authClient.AuthenticateWithFirebaseAsync(tokenRequest, cancellationToken);

            if (response == null || string.IsNullOrEmpty(response.Pxtoken))
            {
                Log("[TokenExchangeApiClient] ERROR: Custom token (Pxtoken) not received from API response");
                throw new InvalidOperationException("Custom token not received from API");
            }

            Log("[TokenExchangeApiClient] Successfully obtained custom token from API");
            return response.Pxtoken;
        }
        catch (ApiException ex)
        {
            Log($"[TokenExchangeApiClient] API exception during token exchange: {ex.StatusCode}");
            Log($"[TokenExchangeApiClient] Error response: {ex.Response}");
            Log($"[TokenExchangeApiClient] Exception message: {ex.Message}");
            
            // Handle the case where API returns 200 with empty body (deserialization error)
            if (ex.StatusCode == 200 && (string.IsNullOrEmpty(ex.Response) || ex.Message.Contains("Could not deserialize")))
            {
                Log("[TokenExchangeApiClient] ERROR: API returned 200 OK but with an empty or invalid response body.");
                Log("[TokenExchangeApiClient] This typically indicates:");
                Log("[TokenExchangeApiClient]   1. The API endpoint is returning an empty response when it should return JSON");
                Log("[TokenExchangeApiClient]   2. The API endpoint may have changed and no longer returns WhoAmIResponse");
                Log("[TokenExchangeApiClient]   3. There may be an authentication or authorization issue causing an empty response");
                
                throw new InvalidOperationException(
                    "Token exchange failed: API returned 200 OK with empty response body. " +
                    "The API endpoint may have changed or there may be a configuration issue. " +
                    "Please check the API documentation for the AuthenticateWithFirebase endpoint.",
                    ex);
            }
            
            throw new InvalidOperationException($"Token exchange failed with status {ex.StatusCode}: {ex.Response}", ex);
        }
        catch (HttpRequestException ex)
        {
            Log($"[TokenExchangeApiClient] HTTP exception: {ex.Message}");
            if (ex.InnerException != null)
            {
                Log($"[TokenExchangeApiClient] Inner exception: {ex.InnerException.Message}");
            }
            throw new InvalidOperationException($"Failed to exchange token: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex)
        {
            Log($"[TokenExchangeApiClient] Request was cancelled: {ex.Message}");
            throw new InvalidOperationException("Token exchange request was cancelled", ex);
        }
        catch (Exception ex)
        {
            Log($"[TokenExchangeApiClient] Exception: {ex.GetType().Name}");
            Log($"[TokenExchangeApiClient] Exception message: {ex.Message}");
            Log($"[TokenExchangeApiClient] Stack trace: {ex.StackTrace}");
            throw;
        }
    }
}

