using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Loudspeaker.ApiClients;
using Loudspeaker.Models;
using Loudspeaker.Services;
using NAudio.Wave;
using ReactiveUI;
using UserModel = Loudspeaker.Models.User;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.ViewModels;

public class MainViewModel : ViewModelBase, IDisposable
{
    private readonly FirebaseAuthService _authService;
    private readonly AuthStateManager _authStateManager;
    private readonly AuthenticatedCarbonVoiceClient _apiClient;
    private readonly HttpClient _httpClient;
    private string _userDisplayName = string.Empty;
    private string _userEmail = string.Empty;
    private bool _isLoadingMessages;
    private string? _messagesError;
    private WaveOutEvent? _currentAudioPlayer;
    private bool _disposed;

    public MainViewModel(
        FirebaseAuthService authService,
        AuthStateManager authStateManager,
        AuthenticatedCarbonVoiceClient apiClient)
    {
        _authService = authService;
        _authStateManager = authStateManager;
        _apiClient = apiClient;
        _httpClient = new HttpClient();

        Messages = new ObservableCollection<MessageV2>();
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
                foreach (var message in messages.OrderByDescending(m=>m.Created_at).Take(10))
                {
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
        _httpClient?.Dispose();
        _disposed = true;
    }
}

