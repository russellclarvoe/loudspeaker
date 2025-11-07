using System;
using System.Reactive.Linq;
using Loudspeaker.ApiClients;
using Loudspeaker.Services;
using Loudspeaker.Views;
using ReactiveUI;

namespace Loudspeaker.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly FirebaseAuthService _authService;
    private readonly AuthStateManager _authStateManager;
    private readonly AuthenticatedCarbonVoiceClient _apiClient;
    private readonly CarbonVoiceWebSocketService _webSocketService;
    private ViewModelBase _currentView;

    public MainWindowViewModel(
        FirebaseAuthService authService,
        AuthStateManager authStateManager,
        AuthenticatedCarbonVoiceClient apiClient,
        CarbonVoiceWebSocketService webSocketService)
    {
        _authService = authService;
        _authStateManager = authStateManager;
        _apiClient = apiClient;
        _webSocketService = webSocketService;

        // Initialize with login view
        _currentView = new LoginViewModel(_authService);

        // Observe authentication state changes
        _authStateManager.WhenAnyValue(x => x.IsAuthenticated)
            .Subscribe(isAuthenticated =>
            {
                CurrentView = isAuthenticated
                    ? new MainViewModel(_authService, _authStateManager, _apiClient, _webSocketService)
                    : new LoginViewModel(_authService);
            });
    }

    public ViewModelBase CurrentView
    {
        get => _currentView;
        private set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }
}

