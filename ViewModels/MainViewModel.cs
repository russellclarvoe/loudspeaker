using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Loudspeaker.ApiClients;
using Loudspeaker.Models;
using Loudspeaker.Services;
using ReactiveUI;
using UserModel = Loudspeaker.Models.User;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly FirebaseAuthService _authService;
    private readonly AuthStateManager _authStateManager;
    private readonly AuthenticatedCarbonVoiceClient _apiClient;
    private string _userDisplayName = string.Empty;
    private string _userEmail = string.Empty;
    private bool _isLoadingMessages;
    private string? _messagesError;

    public MainViewModel(
        FirebaseAuthService authService,
        AuthStateManager authStateManager,
        AuthenticatedCarbonVoiceClient apiClient)
    {
        _authService = authService;
        _authStateManager = authStateManager;
        _apiClient = apiClient;

        Messages = new ObservableCollection<MessageV2>();
        SignOutCommand = ReactiveCommand.Create(SignOut);

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
            // Ensure ReadResponseAsString is set to true (it should already be set by Initialize())
            client.ReadResponseAsString = true;
            
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
                foreach (var message in messages.Take(10))
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

    private void SignOut()
    {
        _authService.SignOut();
    }
}

