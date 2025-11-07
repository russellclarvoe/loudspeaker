using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using LibVLCSharp.Shared;
using Loudspeaker.ApiClients;
using Loudspeaker.Models;
using Loudspeaker.Services;
using NAudio.Wave;
using Newtonsoft.Json;
using ReactiveUI;
using SocketIOClient;
using UserModel = Loudspeaker.Models.User;
using static Loudspeaker.Services.Logger;
using System.Collections.Generic;
using Splat;

namespace Loudspeaker.ViewModels;

public class MainViewModel : ViewModelBase, IDisposable
{
    private readonly FirebaseAuthService _authService;
    private readonly AuthStateManager _authStateManager;
    private readonly AuthenticatedCarbonVoiceClient _apiClient;
    private readonly CarbonVoiceWebSocketService _webSocketService;
    private readonly HttpClient _httpClient;
    private string _userDisplayName = string.Empty;
    private string _userEmail = string.Empty;
    private bool _isLoadingMessages;
    private string? _messagesError;
    private WaveOutEvent? _currentAudioPlayer;
    private LibVLC? _libVlc;
    private MediaPlayer? _hlsPlayer;
    private Media? _currentHlsMedia;
    private bool _disposed;

    public MainViewModel(
        FirebaseAuthService authService,
        AuthStateManager authStateManager,
        AuthenticatedCarbonVoiceClient apiClient,
        CarbonVoiceWebSocketService webSocketService)
    {
        _authService = authService;
        _authStateManager = authStateManager;
        _apiClient = apiClient;
        _webSocketService = webSocketService;
        _httpClient = new HttpClient();

        // Initialize LibVLC for HLS playback
        Core.Initialize();
        
        // Initialize LibVLC with audio output enabled
        // On Windows, we want to use the default audio output
        _libVlc = new LibVLC(enableDebugLogs: false);
        _hlsPlayer = new MediaPlayer(_libVlc);
        
        Log("[MainViewModel] LibVLC initialized");

        Messages = new ObservableCollection<MessageV2>();
        Contacts = new Dictionary<string,Contact>();
        SignOutCommand = ReactiveCommand.Create(SignOut);
        PlayAudioCommand = ReactiveCommand.CreateFromTask<MessageV2>(PlayAudioAsync);

        // Update user info when it changes
        _authStateManager.UserChanged += OnUserChanged;

        // Set initial values
        UpdateUserInfo(_authStateManager.CurrentUser);

        // Fetch messages when authenticated
        if (_authStateManager.IsAuthenticated)
        {
        //    _ = LoadMessagesAsync();
        }

        // Also listen for authentication state changes
        _authStateManager.WhenAnyValue(x => x.IsAuthenticated)
            .Subscribe(isAuthenticated => 
            {
                if (isAuthenticated)
                {
                    _ = LoadMessagesAsync();
                }
                else
                {
                    Messages.Clear();
                    MessagesError = null;
                }
            });

        // Subscribe to Socket.IO events for message updates
        SubscribeToWebSocketEvents();
    }

    private void SubscribeToWebSocketEvents()
    {
        // Subscribe to message:started event
        _webSocketService.On("message:started", response =>
        {
            Log("[MainViewModel] message:started event received");
            _ = HandleMessageStartedAsync(response);
        });

        // Subscribe to message:finished event
        _webSocketService.On("message:finished", response =>
        {
            Log("[MainViewModel] message:finished event received, reloading messages...");
            _ = LoadMessagesAsync();
        });

        // Subscribe to message:finished event
        _webSocketService.On("message:created", response =>
        {
            Log("[MainViewModel] message:created event received");
            _ = HandleMessageCreatedAsync(response);
        });
        Log("[MainViewModel] Subscribed to message:started, message:finished, and message:created events");
    }

    private async Task HandleMessageStartedAsync(SocketIOResponse response)
    {
        try
        {
            Log("[MainViewModel] Processing message:started event...");
            
            // Refresh messages from the API first
            Log("[MainViewModel] Refreshing messages from API...");
            await LoadMessagesAsync();
            
            // Get the newest message from the refreshed list
            var message = Messages
                .OrderByDescending(m => m.Created_at)
                .FirstOrDefault();
            
            if (message != null)
            {
                Log("[MainViewModel] Found newest message");
                
                // Find the audio model with Streaming = true
                var streamingAudioModel = message.Audio_models?.FirstOrDefault(a => a.Streaming);
                
                if (streamingAudioModel != null && !string.IsNullOrEmpty(streamingAudioModel.Url))
                {
                    var m3u8Url = streamingAudioModel.Url;
                    
                    // Add pxtoken to URL if needed
                    var pxtoken = _authStateManager.Pxtoken;
                    if (!string.IsNullOrEmpty(pxtoken) && !m3u8Url.Contains("pxtoken"))
                    {
                        var separator = m3u8Url.Contains('?') ? "&" : "?";
                        m3u8Url = $"{m3u8Url}{separator}pxtoken={Uri.EscapeDataString(pxtoken)}";
                    }
                    
                    Log($"[MainViewModel] Waiting 3 seconds before starting HLS playback for URL: {m3u8Url}");
                    await Task.Delay(3000);
                    Log($"[MainViewModel] Starting HLS playback for URL: {m3u8Url}");
                    await PlayHlsStreamAsync(m3u8Url);
                }
                else
                {
                    Log("[MainViewModel] No streaming audio model found in newest message");
                }
            }
            else
            {
                Log("[MainViewModel] No messages found after refresh");
            }
        }
        catch (Exception ex)
        {
            Log($"[MainViewModel] Error handling message:started event: {ex.Message}");
            if (ex.InnerException != null)
            {
                Log($"[MainViewModel] Inner exception: {ex.InnerException.Message}");
            }
        }
    }

    private async Task HandleMessageCreatedAsync(SocketIOResponse response)
    {
        try
        {
            Log("[MainViewModel] Processing message:created event...");

            // Refresh messages from the API first
            Log("[MainViewModel] Refreshing messages from API...");
            await LoadMessagesAsync();

            // Get the newest message with a non-streaming audio model
            var message = Messages
                .OrderByDescending(m => m.Created_at)
                .FirstOrDefault(m => m.Audio_models?.Any(a => !a.Streaming && !string.IsNullOrEmpty(a.Url)) == true);

            if (message != null)
            {
                Log("[MainViewModel] Found newest message with non-streaming audio, preparing playback");

                // Stop any active HLS playback before starting downloadable audio
                //StopHlsPlayback();

                //await PlayAudioAsync(message);
            }
            else
            {
                Log("[MainViewModel] No non-streaming audio available after refresh");
            }
        }
        catch (Exception ex)
        {
            Log($"[MainViewModel] Error handling message:created event: {ex.Message}");
            if (ex.InnerException != null)
            {
                Log($"[MainViewModel] Inner exception: {ex.InnerException.Message}");
            }
        }
    }

    private async Task PlayHlsStreamAsync(string m3u8Url)
    {
        try
        {
            // Stop any currently playing HLS stream
            StopHlsPlayback();
            
            if (_hlsPlayer == null || _libVlc == null)
            {
                Log("[MainViewModel] HLS player not initialized");
                return;
            }
            
            Log($"[MainViewModel] Creating media from URL: {m3u8Url}");
            
            // Dispose previous media if any
            _currentHlsMedia?.Dispose();
            
            // Create media from the m3u8 URL
            _currentHlsMedia = new Media(_libVlc, m3u8Url, FromType.FromLocation);
            
            // Set up event handlers (only once, but we'll handle duplicates)
            _hlsPlayer.Playing += OnHlsPlaying;
            _hlsPlayer.EndReached += OnHlsEndReached;
            _hlsPlayer.EncounteredError += OnHlsError;
            
            // Set the media on the player first
            _hlsPlayer.Media = _currentHlsMedia;
            
            // Parse the media first to ensure it's ready
            Log("[MainViewModel] Parsing media...");
            try
            {
                await _currentHlsMedia.Parse();
                Log("[MainViewModel] Media parsed successfully");
                
                // Check for audio tracks after parsing
                var tracks = _currentHlsMedia.Tracks;
                Log($"[MainViewModel] Found {tracks?.Count() ?? 0} tracks");
                if (tracks != null)
                {
                    var audioTracks = tracks.Where(t => t.TrackType == TrackType.Audio).ToList();
                    Log($"[MainViewModel] Found {audioTracks.Count} audio tracks");
                    foreach (var track in audioTracks)
                    {
                        Log($"[MainViewModel] Audio track: Codec={track.Codec}, Language={track.Language}, Description={track.Description}");
                    }
                }
            }
            catch (Exception parseEx)
            {
                Log($"[MainViewModel] Media parse warning: {parseEx.Message}");
                // Continue anyway - parsing is optional for playback
            }
            
            // Ensure volume and mute are set before playing
            _hlsPlayer.Mute = false;
            Log($"[MainViewModel] Initial volume: {_hlsPlayer.Volume}, Mute: {_hlsPlayer.Mute}");
            
            // Play the stream
            Log("[MainViewModel] Attempting to play media...");
            var result = _hlsPlayer.Play();
            if (!result)
            {
                Log("[MainViewModel] Failed to start HLS playback - Play() returned false");
            }
            else
            {
                Log($"[MainViewModel] HLS playback started, current state: {_hlsPlayer.State}");
            }
        }
        catch (Exception ex)
        {
            Log($"[MainViewModel] Error playing HLS stream: {ex.Message}");
            if (ex.InnerException != null)
            {
                Log($"[MainViewModel] Inner exception: {ex.InnerException.Message}");
            }
            Log($"[MainViewModel] Stack trace: {ex.StackTrace}");
        }
    }

    private void StopHlsPlayback()
    {
        try
        {
            if (_hlsPlayer != null)
            {
                // Remove event handlers to prevent leaks
                _hlsPlayer.Playing -= OnHlsPlaying;
                _hlsPlayer.EndReached -= OnHlsEndReached;
                _hlsPlayer.EncounteredError -= OnHlsError;
                
                _hlsPlayer.Stop();
                Log("[MainViewModel] Stopped HLS playback");
            }
            
            // Dispose the media
            _currentHlsMedia?.Dispose();
            _currentHlsMedia = null;
        }
        catch (Exception ex)
        {
            Log($"[MainViewModel] Error stopping HLS playback: {ex.Message}");
        }
    }

    private async void OnHlsPlaying(object? sender, EventArgs e)
    {
        if (_hlsPlayer != null)
        {
            Log($"[MainViewModel] HLS stream started playing - State: {_hlsPlayer.State}, Volume: {_hlsPlayer.Volume}, Mute: {_hlsPlayer.Mute}");
            
            // Wait a bit for the media to fully load before setting volume
            // HLS streams need time to download the first segment
            await Task.Delay(500);
            
            // Try to set volume multiple times if needed (volume might not be settable immediately)
            for (int i = 0; i < 5; i++)
            {
                _hlsPlayer.Volume = 100;
                _hlsPlayer.Mute = false;
                var currentVolume = _hlsPlayer.Volume;
                Log($"[MainViewModel] Attempt {i + 1}: Set volume to 100, actual volume: {currentVolume}");
                
                if (currentVolume >= 0)
                {
                    Log($"[MainViewModel] Volume successfully set to {currentVolume}");
                    break;
                }
                
                await Task.Delay(200);
            }
            
            // Check audio tracks after a delay (HLS tracks might not be available immediately)
            await Task.Delay(1000);
            
            if (_currentHlsMedia != null)
            {
                try
                {
                    // Re-parse to get tracks if they weren't available before
                    await _currentHlsMedia.Parse();
                    
                    var tracks = _currentHlsMedia.Tracks;
                    Log($"[MainViewModel] Media tracks after playback: {tracks?.Count() ?? 0}");
                    
                    if (tracks != null)
                    {
                        var audioTracks = tracks.Where(t => t.TrackType == TrackType.Audio).ToList();
                        Log($"[MainViewModel] Available audio tracks: {audioTracks.Count}");
                        
                        foreach (var track in audioTracks)
                        {
                            Log($"[MainViewModel] Audio track: Id={track.Id}, Codec={track.Codec}, Language={track.Language}, Description={track.Description}");
                        }
                        
                        // Try to set audio track if available
                        if (audioTracks.Count > 0)
                        {
                            var audioTrackId = audioTracks[0].Id;
                            _hlsPlayer.SetAudioTrack(audioTrackId);
                            Log($"[MainViewModel] Set audio track to ID: {audioTrackId}");
                        }
                        else
                        {
                            Log("[MainViewModel] WARNING: No audio tracks found in HLS stream!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"[MainViewModel] Error checking tracks: {ex.Message}");
                }
            }
            
            // Final volume check
            Log($"[MainViewModel] Final state - Volume: {_hlsPlayer.Volume}, Mute: {_hlsPlayer.Mute}, State: {_hlsPlayer.State}");
        }
    }

    private void OnHlsEndReached(object? sender, EventArgs e)
    {
        Log("[MainViewModel] HLS stream ended");
    }

    private void OnHlsError(object? sender, EventArgs e)
    {
        Log($"[MainViewModel] HLS player error occurred - State: {_hlsPlayer?.State}");
    }


    private void OnUserChanged(object? sender, UserModel? user)
    {
        UpdateUserInfo(user);
    }

    private void UpdateUserInfo(UserModel? user)
    {
        if (user != null)
        {
            UserDisplayName = user.DisplayName ?? user.Email ?? "User";
            UserEmail = user.Email ?? string.Empty;
        }
        else
        {
            UserDisplayName = string.Empty;
            UserEmail = string.Empty;
        }
    }

    private async Task<Contact?> GetContactAsync(string userId)
    {
        if (Contacts.TryGetValue(userId, out var ct))
        {
            return ct;
        }


        try
        {
            var client = _apiClient.GetUserClient();
            var contacts = await client.GetMyContactsAsync(new UserSearchParameters() { User_guids = new List<string> { userId } });

            if (contacts != null)
            {
                foreach (var contact in contacts)
                {
                    Contacts[contact.User_guid] = contact;
                }
            }
        }
        catch (Exception e)
        {
            var exception = e;
            while (exception != null)
            {
                Log(exception.Message);
                if (exception.StackTrace != null)
                {
                    Log(exception.StackTrace);
                }
                exception = exception.InnerException;
            }
        }
            
            if (Contacts.TryGetValue(userId, out var ct2))
        {
            return ct2;
        }
        return null;
    }

    private async Task LoadMessagesAsync()
    {
        IsLoadingMessages = true;
        MessagesError = null;



        try
        {
            Log("[MainViewModel] Loading recent messages...");
            var client = _apiClient.GetRecentMessagesV3Client();


            var queryParams = new MessageQueryParameters
            {
                Date = DateTimeOffset.UtcNow,
                Direction = MessageQueryParametersDirection.Older,
                Limit = 10,
                Use_last_updated = false
            };

            var messages = await client.V3Async(queryParams);

            Messages.Clear();
            if (messages != null)
            {
                // Take only the 10 most recent messages
                foreach (var message in messages.OrderByDescending(m => m.Created_at).Take(10))
                {
                    message.CreatorContact = await GetContactAsync(message.Creator_id);

                    Messages.Add(message);
                }
                Log($"[MainViewModel] Loaded {Messages.Count} messages");
            }
        }
        catch (Exception ex)
        {
            Log($"[MainViewModel] Error loading messages: {ex.GetType().Name}");
            Log($"[MainViewModel] Error message: {ex.Message}");
            MessagesError = $"Failed to load messages: {ex.Message}";
        }
        finally
        {
            IsLoadingMessages = false;
        }
    }

    public ICommand SignOutCommand { get; }

    public ObservableCollection<MessageV2> Messages { get; }

    public Dictionary<string,Contact> Contacts{ get; }

    public string UserDisplayName
    {
        get => _userDisplayName;
        set => this.RaiseAndSetIfChanged(ref _userDisplayName, value);
    }

    public string UserEmail
    {
        get => _userEmail;
        set => this.RaiseAndSetIfChanged(ref _userEmail, value);
    }

    public bool IsLoadingMessages
    {
        get => _isLoadingMessages;
        set => this.RaiseAndSetIfChanged(ref _isLoadingMessages, value);
    }

    public string? MessagesError
    {
        get => _messagesError;
        set
        {
            this.RaiseAndSetIfChanged(ref _messagesError, value);
            this.RaisePropertyChanged(nameof(HasMessagesError));
        }
    }

    public bool HasMessagesError => !string.IsNullOrEmpty(_messagesError);

    public string? Pxtoken => _authStateManager.Pxtoken;

    public ICommand PlayAudioCommand { get; }

    private void SignOut()
    {
        _authService.SignOut();
    }

    private async Task PlayAudioAsync(MessageV2 message)
    {
        if (message == null)
            return;

        var audioModel = message.Audio_models?.FirstOrDefault(a => !a.Streaming);
        if (audioModel == null || string.IsNullOrEmpty(audioModel.Url))
        {
            Log("[MainViewModel] No non-streaming audio model found for message");
            return;
        }

        var pxtoken = _authStateManager.Pxtoken;
        if (string.IsNullOrEmpty(pxtoken))
        {
            Log("[MainViewModel] Cannot play audio: pxtoken is not available");
            return;
        }

        try
        {
            // Stop any currently playing audio
            StopCurrentAudio();

            var url = audioModel.Url;
            var separator = url.Contains('?') ? "&" : "?";
            var urlWithToken = $"{url}{separator}pxtoken={Uri.EscapeDataString(pxtoken)}";
            
            Log($"[MainViewModel] Downloading audio from: {urlWithToken}");
            
            // Download the audio file
            var audioBytes = await _httpClient.GetByteArrayAsync(urlWithToken);
            
            Log($"[MainViewModel] Audio downloaded ({audioBytes.Length} bytes), starting playback...");
            
            // Play the audio using NAudio
            await PlayAudioBytesAsync(audioBytes);
        }
        catch (Exception ex)
        {
            Log($"[MainViewModel] Error playing audio: {ex.Message}");
            if (ex.InnerException != null)
            {
                Log($"[MainViewModel] Inner exception: {ex.InnerException.Message}");
            }
        }
    }

    private async Task PlayAudioBytesAsync(byte[] audioBytes)
    {
        MemoryStream? audioStream = null;
        Mp3FileReader? mp3Reader = null;
        
        try
        {
            // Create a memory stream from the audio bytes
            audioStream = new MemoryStream(audioBytes);
            
            // Create MP3 file reader
            mp3Reader = new Mp3FileReader(audioStream);
            
            // Create wave output device
            _currentAudioPlayer = new WaveOutEvent();
            _currentAudioPlayer.Init(mp3Reader);
            
            Log("[MainViewModel] Starting audio playback...");
            _currentAudioPlayer.Play();
            
            // Wait for playback to complete
            while (_currentAudioPlayer.PlaybackState == PlaybackState.Playing)
            {
                await Task.Delay(100);
            }
            
            Log("[MainViewModel] Audio playback completed");
        }
        finally
        {
            // Clean up - dispose in reverse order
            if (_currentAudioPlayer != null)
            {
                _currentAudioPlayer.Dispose();
                _currentAudioPlayer = null;
            }
            
            mp3Reader?.Dispose();
            audioStream?.Dispose();
        }
    }

    private void StopCurrentAudio()
    {
        if (_currentAudioPlayer != null)
        {
            try
            {
                _currentAudioPlayer.Stop();
                _currentAudioPlayer.Dispose();
                Log("[MainViewModel] Stopped previous audio playback");
            }
            catch (Exception ex)
            {
                Log($"[MainViewModel] Error stopping audio: {ex.Message}");
            }
            finally
            {
                _currentAudioPlayer = null;
            }
        }
    }

    public static string? GetTranscriptText(MessageV2? message)
    {
        if (message?.Text_models == null)
            return null;

        var transcriptModel = message.Text_models.FirstOrDefault(tm => tm.Type == TextModelType.Transcript);
        return transcriptModel?.Value;
    }

    public static bool HasAudioToPlay(MessageV2? message)
    {
        if (message?.Audio_models == null)
            return false;

        return message.Audio_models.Any(a => !a.Streaming);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        StopCurrentAudio();
        StopHlsPlayback();
        
        _currentHlsMedia?.Dispose();
        _hlsPlayer?.Dispose();
        _libVlc?.Dispose();
        _httpClient?.Dispose();
        _disposed = true;
    }
}

