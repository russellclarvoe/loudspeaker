using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Loudspeaker.Services;
using ReactiveUI;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly FirebaseAuthService _authService;
    private bool _isLoading;
    private string? _errorMessage;
    private bool _hasError;

    public LoginViewModel(FirebaseAuthService authService)
    {
        _authService = authService;
        SignInCommand = ReactiveCommand.CreateFromTask(SignInAsync);
        OpenLogFileCommand = ReactiveCommand.Create(OpenLogFile);
    }

    public ICommand SignInCommand { get; }
    public ICommand OpenLogFileCommand { get; }

    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    public bool HasError
    {
        get => _hasError;
        set => this.RaiseAndSetIfChanged(ref _hasError, value);
    }

    private async Task SignInAsync()
    {
        IsLoading = true;
        HasError = false;
        ErrorMessage = null;

        try
        {
            Log("[LoginViewModel] Sign-in button clicked, starting authentication...");
            var success = await _authService.SignInWithGoogleAsync();
            if (!success)
            {
                Log("[LoginViewModel] Sign-in returned false");
                var logPath = GetLogFilePath();
                var recentErrors = GetRecentErrors(logPath);
                
                var errorMsg = "Failed to sign in.";
                if (!string.IsNullOrEmpty(recentErrors))
                {
                    errorMsg += $"\n\nRecent errors:\n{recentErrors}";
                }
                errorMsg += $"\n\nLog file: {logPath}";
                
                ErrorMessage = errorMsg;
                HasError = true;
            }
            else
            {
                Log("[LoginViewModel] Sign-in completed successfully");
            }
        }
        catch (NotImplementedException ex)
        {
            Log($"[LoginViewModel] NotImplementedException: {ex.Message}");
            var logPath = GetLogFilePath();
            ErrorMessage = $"OAuth flow not yet implemented: {ex.Message}\n\nLog file: {logPath}";
            HasError = true;
        }
        catch (Exception ex)
        {
            Log($"[LoginViewModel] Exception during sign-in: {ex.GetType().Name}");
            Log($"[LoginViewModel] Exception message: {ex.Message}");
            Log($"[LoginViewModel] Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Log($"[LoginViewModel] Inner exception: {ex.InnerException.Message}");
            }
            
            // Provide more detailed error message to user
            var errorMsg = $"An error occurred: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMsg += $" ({ex.InnerException.Message})";
            }
            
            var logPath = GetLogFilePath();
            var recentErrors = GetRecentErrors(logPath);
            if (!string.IsNullOrEmpty(recentErrors))
            {
                errorMsg += $"\n\nRecent errors:\n{recentErrors}";
            }
            errorMsg += $"\n\nLog file: {logPath}";
            
            ErrorMessage = errorMsg;
            HasError = true;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private string GetRecentErrors(string logPath)
    {
        try
        {
            if (!File.Exists(logPath))
            {
                return string.Empty;
            }

            var lines = File.ReadAllLines(logPath);
            // Get the last 10 lines that contain "ERROR"
            var errorLines = lines
                .Where(line => line.Contains("ERROR", StringComparison.OrdinalIgnoreCase))
                .TakeLast(10)
                .ToList();

            if (errorLines.Count == 0)
            {
                // If no ERROR lines, get the last 5 lines
                errorLines = lines.TakeLast(5).ToList();
            }

            return string.Join("\n", errorLines);
        }
        catch
        {
            return string.Empty;
        }
    }

    private void OpenLogFile()
    {
        try
        {
            var logPath = GetLogFilePath();
            var logDir = Path.GetDirectoryName(logPath);
            
            if (!string.IsNullOrEmpty(logDir) && Directory.Exists(logDir))
            {
                // Open the log directory in Windows Explorer
                Process.Start(new ProcessStartInfo
                {
                    FileName = logDir,
                    UseShellExecute = true
                });
            }
            else if (File.Exists(logPath))
            {
                // If directory doesn't exist but file does, try to open the file
                Process.Start(new ProcessStartInfo
                {
                    FileName = logPath,
                    UseShellExecute = true
                });
            }
        }
        catch (Exception ex)
        {
            Log($"[LoginViewModel] Failed to open log file: {ex.Message}");
        }
    }
}

