using System;
using Loudspeaker.Models;
using ReactiveUI;

namespace Loudspeaker.Services;

public class AuthStateManager : ReactiveObject
{
    private User? _currentUser;
    private string? _idToken;
    private string? _customToken;
    private string? _pxtoken;
    private bool _isAuthenticated;

    public User? CurrentUser
    {
        get => _currentUser;
        private set => this.RaiseAndSetIfChanged(ref _currentUser, value);
    }

    public string? IdToken
    {
        get => _idToken;
        private set => this.RaiseAndSetIfChanged(ref _idToken, value);
    }

    public string? CustomToken
    {
        get => _customToken;
        private set => this.RaiseAndSetIfChanged(ref _customToken, value);
    }

    public string? Pxtoken
    {
        get => _pxtoken;
        private set => this.RaiseAndSetIfChanged(ref _pxtoken, value);
    }

    public bool IsAuthenticated
    {
        get => _isAuthenticated;
        private set => this.RaiseAndSetIfChanged(ref _isAuthenticated, value);
    }

    public event EventHandler<User?>? UserChanged;
    public event EventHandler<bool>? AuthenticationStateChanged;

    public void SetAuthenticatedUser(User user, string idToken, string customToken)
    {
        CurrentUser = user;
        IdToken = idToken;
        CustomToken = customToken;
        IsAuthenticated = true;
        UserChanged?.Invoke(this, user);
        AuthenticationStateChanged?.Invoke(this, true);
    }

    public void SetPxtoken(string pxtoken)
    {
        Pxtoken = pxtoken;
    }

    public void ClearAuthentication()
    {
        CurrentUser = null;
        IdToken = null;
        CustomToken = null;
        Pxtoken = null;
        IsAuthenticated = false;
        UserChanged?.Invoke(this, null);
        AuthenticationStateChanged?.Invoke(this, false);
    }
}

