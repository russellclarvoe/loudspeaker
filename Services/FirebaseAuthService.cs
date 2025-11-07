using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using IdentityModel.OidcClient;
using IdentityModel.Jwk;
using Loudspeaker.Config;
using Loudspeaker.Models;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.Services;

public class FirebaseAuthService
{
    private readonly AppConfig _config;
    private readonly TokenExchangeApiClient _apiClient;
    private readonly AuthStateManager _authStateManager;
    private readonly CarbonVoiceAuthService _carbonVoiceAuthService;
    private readonly HttpClient _httpClient;

    public FirebaseAuthService(
        AppConfig config,
        TokenExchangeApiClient apiClient,
        AuthStateManager authStateManager,
        CarbonVoiceAuthService carbonVoiceAuthService)
    {
        _config = config;
        _apiClient = apiClient;
        _authStateManager = authStateManager;
        _carbonVoiceAuthService = carbonVoiceAuthService;
        _httpClient = new HttpClient();
    }

    public async Task<bool> SignInWithGoogleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Log("[FirebaseAuthService] Starting Google sign-in process...");
            
            // Step 1: Get Google OAuth token
            Log("[FirebaseAuthService] Step 1: Getting Google OAuth token...");
            var googleIdToken = await GetGoogleIdTokenAsync(cancellationToken);
            if (string.IsNullOrEmpty(googleIdToken))
            {
                Log("[FirebaseAuthService] ERROR: Failed to get Google OAuth token");
                return false;
            }
            Log("[FirebaseAuthService] Successfully obtained Google OAuth token");

            // Step 2: Sign in to Firebase with Google token
            Log("[FirebaseAuthService] Step 2: Signing in to Firebase with Google token...");
            var firebaseIdToken = await SignInToFirebaseWithGoogleAsync(googleIdToken, cancellationToken);
            if (string.IsNullOrEmpty(firebaseIdToken))
            {
                Log("[FirebaseAuthService] ERROR: Failed to sign in to Firebase with Google token");
                return false;
            }
            Log("[FirebaseAuthService] Successfully signed in to Firebase");

            // Step 3: Exchange Firebase ID token for custom token via API
            Log("[FirebaseAuthService] Step 3: Exchanging Firebase ID token for custom token...");
            string customToken;
            try
            {
                customToken = await _apiClient.ExchangeIdTokenForCustomTokenAsync(firebaseIdToken, cancellationToken);
                Log("[FirebaseAuthService] Successfully obtained custom token");
            }
            catch (Exception ex)
            {
                Log($"[FirebaseAuthService] ERROR: Failed to exchange token: {ex.Message}");
                Log($"[FirebaseAuthService] Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Log($"[FirebaseAuthService] Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }

            // Step 4: Get user information
            Log("[FirebaseAuthService] Step 4: Getting user information...");
            var user = await GetUserInfoAsync(firebaseIdToken, cancellationToken);
            if (user == null)
            {
                Log("[FirebaseAuthService] ERROR: Failed to get user information");
                return false;
            }
            Log($"[FirebaseAuthService] Successfully retrieved user info for: {user.Email}");

            // Step 5: Store customToken as pxtoken for API access
            Log("[FirebaseAuthService] Step 5: Storing custom token as pxtoken...");
            _carbonVoiceAuthService.SetPxtoken(customToken);
            _authStateManager.SetPxtoken(customToken);
            Log("[FirebaseAuthService] Successfully stored pxtoken");

            // Step 6: Set authenticated user in AuthStateManager (this triggers view navigation)
            Log("[FirebaseAuthService] Step 6: Setting authenticated user state...");
            _authStateManager.SetAuthenticatedUser(user, firebaseIdToken, customToken);
            Log("[FirebaseAuthService] Successfully set authenticated user state");
            
            return true;
        }
        catch (Exception ex)
        {
            Log($"[FirebaseAuthService] FATAL ERROR: Authentication failed");
            Log($"[FirebaseAuthService] Error message: {ex.Message}");
            Log($"[FirebaseAuthService] Error type: {ex.GetType().Name}");
            Log($"[FirebaseAuthService] Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Log($"[FirebaseAuthService] Inner exception: {ex.InnerException.Message}");
                Log($"[FirebaseAuthService] Inner exception type: {ex.InnerException.GetType().Name}");
            }
            return false;
        }
    }

    private async Task<string?> GetGoogleIdTokenAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Use Google OAuth 2.0 flow
            // For desktop apps, we use the system browser with a local redirect URI
            var clientId = _config.GoogleOAuthClientId;
            if (string.IsNullOrEmpty(clientId))
            {
                Log("[FirebaseAuthService] WARNING: GoogleOAuthClientId is not configured, using FirebaseApiKey as fallback");
                // Fallback: try using Firebase API key (may not work for all scenarios)
                clientId = _config.FirebaseApiKey;
            }
            else
            {
                Log($"[FirebaseAuthService] Using GoogleOAuthClientId: {clientId.Substring(0, Math.Min(20, clientId.Length))}...");
            }

            // Manually configure Google OAuth endpoints to avoid host validation issues
            // Google's discovery document has endpoints on different hosts (oauth2.googleapis.com)
            // which causes validation errors in the OIDC client
            var options = new OidcClientOptions
            {
                Authority = "https://accounts.google.com",
                ClientId = clientId,
                Scope = "openid profile email",
                RedirectUri = "http://127.0.0.1:7890/",
                Browser = new SystemBrowser(port: 7890),
                LoadProfile = false // Don't load discovery document, use manual configuration
            };

            // If a client secret is provided (e.g., using a Web client), pass it through.
            // Not recommended for desktop apps, but supported as an escape hatch.
            if (!string.IsNullOrWhiteSpace(_config.GoogleOAuthClientSecret))
            {
                options.ClientSecret = _config.GoogleOAuthClientSecret;
                Log("[FirebaseAuthService] Using Google OAuth client secret from configuration");
            }

            // OIDC client will use PKCE automatically; no client secret needed for Desktop client IDs

            // Manually set the provider information with Google's actual endpoints
            options.ProviderInformation = new IdentityModel.OidcClient.ProviderInformation
            {
                IssuerName = "https://accounts.google.com",
                AuthorizeEndpoint = "https://accounts.google.com/o/oauth2/v2/auth",
                TokenEndpoint = "https://oauth2.googleapis.com/token",
                UserInfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo",
                EndSessionEndpoint = "https://oauth2.googleapis.com/revoke"
            };

            // Provide Google's JWKS so the client can validate tokens without discovery
            try
            {
                var jwksJson = await _httpClient.GetStringAsync("https://www.googleapis.com/oauth2/v3/certs", cancellationToken);
                options.ProviderInformation.KeySet = new JsonWebKeySet(jwksJson);
                Log("[FirebaseAuthService] Loaded Google JWKS key set successfully");
            }
            catch (Exception ex)
            {
                Log($"[FirebaseAuthService] WARNING: Failed to load Google JWKS: {ex.Message}");
            }

            Log("[FirebaseAuthService] Initializing OIDC client...");
            var oidcClient = new OidcClient(options);

            Log("[FirebaseAuthService] Starting OAuth login flow...");
            var result = await oidcClient.LoginAsync(cancellationToken: cancellationToken);
            
            if (result.IsError)
            {
                Log($"[FirebaseAuthService] OAuth error: {result.Error}");
                if (!string.IsNullOrEmpty(result.ErrorDescription))
                {
                    Log($"[FirebaseAuthService] Error description: {result.ErrorDescription}");
                }
                if (!string.IsNullOrEmpty(result.Error))
                {
                    Log($"[FirebaseAuthService] Error code: {result.Error}");
                }
                return null;
            }

            if (string.IsNullOrEmpty(result.IdentityToken))
            {
                Log("[FirebaseAuthService] ERROR: OAuth login succeeded but IdentityToken is null or empty");
                return null;
            }

            Log("[FirebaseAuthService] Successfully obtained Google ID token");
            // Extract ID token from the result
            return result.IdentityToken;
        }
        catch (Exception ex)
        {
            Log($"[FirebaseAuthService] Exception during OAuth flow: {ex.GetType().Name}");
            Log($"[FirebaseAuthService] Exception message: {ex.Message}");
            Log($"[FirebaseAuthService] Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Log($"[FirebaseAuthService] Inner exception: {ex.InnerException.Message}");
            }
            return null;
        }
    }

    private async Task<string?> SignInToFirebaseWithGoogleAsync(string googleIdToken, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={_config.FirebaseApiKey}";
            Log($"[FirebaseAuthService] Calling Firebase signInWithIdp endpoint...");
            
            var request = new
            {
                postBody = $"id_token={Uri.EscapeDataString(googleIdToken)}&providerId=google.com",
                requestUri = "http://localhost",
                returnIdpCredential = true,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                Log($"[FirebaseAuthService] ERROR: Firebase signInWithIdp failed with status {response.StatusCode}");
                Log($"[FirebaseAuthService] Error response: {errorContent}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<FirebaseSignInResponse>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken);

            if (result == null || string.IsNullOrEmpty(result.IdToken))
            {
                Log("[FirebaseAuthService] ERROR: Firebase signInWithIdp response is null or missing IdToken");
                return null;
            }

            return result.IdToken;
        }
        catch (HttpRequestException ex)
        {
            Log($"[FirebaseAuthService] HTTP exception during Firebase sign-in: {ex.Message}");
            if (ex.InnerException != null)
            {
                Log($"[FirebaseAuthService] Inner exception: {ex.InnerException.Message}");
            }
            return null;
        }
        catch (Exception ex)
        {
            Log($"[FirebaseAuthService] Exception during Firebase sign-in: {ex.GetType().Name}");
            Log($"[FirebaseAuthService] Exception message: {ex.Message}");
            Log($"[FirebaseAuthService] Stack trace: {ex.StackTrace}");
            return null;
        }
    }

    private async Task<bool> SignInWithCustomTokenAsync(string customToken, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={_config.FirebaseApiKey}";
            Log($"[FirebaseAuthService] Calling Firebase signInWithCustomToken endpoint...");
            
            var request = new
            {
                token = customToken,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                Log($"[FirebaseAuthService] ERROR: Custom token sign-in failed with status {response.StatusCode}");
                Log($"[FirebaseAuthService] Error response: {errorContent}");
                return false;
            }

            var result = await response.Content.ReadFromJsonAsync<FirebaseSignInResponse>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken);

            if (result == null || string.IsNullOrEmpty(result.IdToken))
            {
                Log("[FirebaseAuthService] ERROR: Custom token sign-in response is null or missing IdToken");
                return false;
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            Log($"[FirebaseAuthService] HTTP exception during custom token sign-in: {ex.Message}");
            if (ex.InnerException != null)
            {
                Log($"[FirebaseAuthService] Inner exception: {ex.InnerException.Message}");
            }
            return false;
        }
        catch (Exception ex)
        {
            Log($"[FirebaseAuthService] Exception during custom token sign-in: {ex.GetType().Name}");
            Log($"[FirebaseAuthService] Exception message: {ex.Message}");
            Log($"[FirebaseAuthService] Stack trace: {ex.StackTrace}");
            return false;
        }
    }

    private async Task<User?> GetUserInfoAsync(string idToken, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:lookup?key={_config.FirebaseApiKey}";
            Log($"[FirebaseAuthService] Calling Firebase accounts:lookup endpoint...");
            
            var request = new
            {
                idToken = idToken
            };

            var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                Log($"[FirebaseAuthService] ERROR: GetUserInfo failed with status {response.StatusCode}");
                Log($"[FirebaseAuthService] Error response: {errorContent}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<FirebaseUserInfoResponse>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken);

            if (result?.Users == null || result.Users.Length == 0)
            {
                Log("[FirebaseAuthService] WARNING: GetUserInfo returned no users");
                return null;
            }

            var userInfo = result.Users[0];
            Log($"[FirebaseAuthService] Successfully retrieved user info for: {userInfo.Email}");
            return new User
            {
                Uid = userInfo.LocalId ?? string.Empty,
                Email = userInfo.Email ?? string.Empty,
                DisplayName = userInfo.DisplayName ?? string.Empty,
                PhotoUrl = userInfo.PhotoUrl ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            Log($"[FirebaseAuthService] Exception during GetUserInfo: {ex.GetType().Name}");
            Log($"[FirebaseAuthService] Exception message: {ex.Message}");
            return null;
        }
    }

    public void SignOut()
    {
        _authStateManager.ClearAuthentication();
    }

    private class FirebaseSignInResponse
    {
        public string? IdToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? ExpiresIn { get; set; }
    }

    private class FirebaseUserInfoResponse
    {
        public UserInfo[]? Users { get; set; }
    }

    private class UserInfo
    {
        public string? LocalId { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? PhotoUrl { get; set; }
    }
}

