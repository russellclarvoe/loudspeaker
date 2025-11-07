using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Loudspeaker.ApiClients;
using Loudspeaker.Services;
using Loudspeaker.ViewModels;
using Loudspeaker.Views;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Loudspeaker;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var appConfig = new Config.AppConfig(configuration);

            // Initialize services
            var tokenExchangeApiClient = new TokenExchangeApiClient(appConfig);
            var authStateManager = new AuthStateManager();
            
            // Initialize CarbonVoice API services (needed by FirebaseAuthService)
            var carbonVoiceAuthService = new CarbonVoiceAuthService(appConfig);
            var authenticatedCarbonVoiceClient = new AuthenticatedCarbonVoiceClient(carbonVoiceAuthService, appConfig.ApiBaseUrl);
            
            // Initialize FirebaseAuthService (depends on CarbonVoiceAuthService)
            var firebaseAuthService = new FirebaseAuthService(appConfig, tokenExchangeApiClient, authStateManager, carbonVoiceAuthService);

            // Create main window
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(firebaseAuthService, authStateManager, authenticatedCarbonVoiceClient)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}

