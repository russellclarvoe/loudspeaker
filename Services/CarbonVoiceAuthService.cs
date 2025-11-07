using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Loudspeaker.ApiClients;
using Loudspeaker.Config;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.Services;

public class CarbonVoiceAuthService
{
    private readonly AppConfig _config;
    private readonly HttpClient _httpClient;
    private readonly AuthControllerClient _authClient;
    private string? _pxtoken;

    public CarbonVoiceAuthService(AppConfig config)
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

    public async Task<string?> LoginAsync(string firebaseIdToken, CancellationToken cancellationToken = default)
    {
        try
        {
            Log($"[CarbonVoiceAuthService] Logging in with Firebase ID token at {_config.ApiBaseUrl}");

            var tokenRequest = new TokenRequest
            {
                IdToken = firebaseIdToken
            };

            var response = await _authClient.LoginWithFirebaseAsync(tokenRequest, cancellationToken);

            if (response == null || string.IsNullOrEmpty(response.Pxtoken))
            {
                Log("[CarbonVoiceAuthService] ERROR: Login response is null or missing Pxtoken");
                return null;
            }

            _pxtoken = response.Pxtoken;
            Log("[CarbonVoiceAuthService] Successfully obtained pxtoken");
            return _pxtoken;
        }
        catch (ApiException ex)
        {
            Log($"[CarbonVoiceAuthService] API exception during login: {ex.StatusCode}");
            Log($"[CarbonVoiceAuthService] Error response: {ex.Response}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Log($"[CarbonVoiceAuthService] HTTP exception during login: {ex.Message}");
            if (ex.InnerException != null)
            {
                Log($"[CarbonVoiceAuthService] Inner exception: {ex.InnerException.Message}");
            }
            return null;
        }
        catch (Exception ex)
        {
            Log($"[CarbonVoiceAuthService] Exception during login: {ex.GetType().Name}");
            Log($"[CarbonVoiceAuthService] Exception message: {ex.Message}");
            Log($"[CarbonVoiceAuthService] Stack trace: {ex.StackTrace}");
            return null;
        }
    }

    public string? GetPxtoken()
    {
        return _pxtoken;
    }

    public void SetPxtoken(string pxtoken)
    {
        _pxtoken = pxtoken;
        Log("[CarbonVoiceAuthService] Set pxtoken");
    }

    public void ClearPxtoken()
    {
        _pxtoken = null;
        Log("[CarbonVoiceAuthService] Cleared pxtoken");
    }
}

