using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using SocketIOClient;
using SocketIOClient.Transport;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.Services;

public class CarbonVoiceWebSocketService : IDisposable
{
    private readonly AuthStateManager _authStateManager;
    private SocketIO? _socket;
    private bool _isConnected;
    private bool _disposed;
    private IDisposable? _pxtokenSubscription;
    private readonly List<(string eventName, Action<SocketIOResponse> handler)> _eventHandlers = new();

    public bool IsConnected
    {
        get => _isConnected;
        private set
        {
            if (_isConnected != value)
            {
                _isConnected = value;
                Log($"[CarbonVoiceWebSocketService] Connection state changed: {value}");
            }
        }
    }

    public CarbonVoiceWebSocketService(AuthStateManager authStateManager)
    {
        _authStateManager = authStateManager;
        
        // Connect immediately if pxtoken is already available
        if (!string.IsNullOrEmpty(_authStateManager.Pxtoken))
        {
            Task.Run(async () =>
            {
                try
                {
                    await ConnectAsync();
                }
                catch (Exception ex)
                {
                    Log($"[CarbonVoiceWebSocketService] Failed to auto-connect on initialization: {ex.Message}");
                }
            });
        }
        
        // Automatically connect when pxtoken becomes available
        _pxtokenSubscription = _authStateManager.WhenAnyValue(x => x.Pxtoken)
            .Where(pxtoken => !string.IsNullOrEmpty(pxtoken))
            .Subscribe(async pxtoken =>
            {
                if (!IsConnected)
                {
                    try
                    {
                        await ConnectAsync();
                    }
                    catch (Exception ex)
                    {
                        Log($"[CarbonVoiceWebSocketService] Failed to auto-connect: {ex.Message}");
                    }
                }
            });
        
        // Disconnect when pxtoken is cleared
        _authStateManager.WhenAnyValue(x => x.Pxtoken)
            .Where(pxtoken => string.IsNullOrEmpty(pxtoken))
            .Subscribe(async _ =>
            {
                if (IsConnected)
                {
                    await DisconnectAsync();
                }
            });
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (_socket != null && _socket.Connected)
        {
            Log("[CarbonVoiceWebSocketService] Already connected");
            return;
        }

        var pxtoken = _authStateManager.Pxtoken;
        if (string.IsNullOrEmpty(pxtoken))
        {
            Log("[CarbonVoiceWebSocketService] ERROR: Cannot connect - pxtoken is not available");
            throw new InvalidOperationException("pxtoken is required to connect to WebSocket");
        }

        try
        {
            Log("[CarbonVoiceWebSocketService] Connecting to Socket.IO server at wss://ws.carbonvoice.app...");
            
            // Disconnect existing socket if any
            if (_socket != null)
            {
                await _socket.DisconnectAsync();
                _socket.Dispose();
            }
            
            // Create Socket.IO client with options
            var options = new SocketIOOptions
            {
                Transport = TransportProtocol.WebSocket,
                AutoUpgrade = true, // Automatically upgrade from polling to WebSocket
                Reconnection = true,
                ReconnectionDelay = 1000,
                ReconnectionDelayMax = 5000
            };
            
            // Set the pxtoken as an extra header (Socket.IO supports headers)
            options.ExtraHeaders = new Dictionary<string, string>
            {
                { "pxtoken", pxtoken }
            };
            
            // Also set as query parameter (some Socket.IO servers prefer this)
            // The library will append this to the connection URL
            options.Query = new Dictionary<string, string>
            {
                { "pxtoken", pxtoken }
            };
            
            // For Socket.IO, use the base URL - the library handles the /socket.io/ path
            // Use wss:// explicitly, or https:// (the library will use the appropriate protocol)
            _socket = new SocketIO("wss://ws.carbonvoice.app", options);
            
            // Register all event handlers (both pending and previously registered ones)
            foreach (var (eventName, handler) in _eventHandlers)
            {
                _socket.On(eventName, handler);
                Log($"[CarbonVoiceWebSocketService] Registered handler for event: {eventName}");
            }
            
            // Set up event handlers
            _socket.OnConnected += (sender, e) =>
            {
                IsConnected = true;
                Log("[CarbonVoiceWebSocketService] Successfully connected to Socket.IO server");
            };
            
            _socket.OnDisconnected += (sender, e) =>
            {
                IsConnected = false;
                Log($"[CarbonVoiceWebSocketService] Disconnected from Socket.IO server: {e}");
            };
            
            _socket.OnError += (sender, e) =>
            {
                Log($"[CarbonVoiceWebSocketService] Socket.IO error: {e}");
            };
            
            // Handle any event (catch-all for messages)
            _socket.OnAny((eventName, response) =>
            {
                Log($"[CarbonVoiceWebSocketService] Event received: {eventName}, Data: {response}");
            });
            
            // Connect to the server
            await _socket.ConnectAsync();
        }
        catch (Exception ex)
        {
            Log($"[CarbonVoiceWebSocketService] ERROR: Failed to connect: {ex.GetType().Name} - {ex.Message}");
            if (ex.InnerException != null)
            {
                Log($"[CarbonVoiceWebSocketService] Inner exception: {ex.InnerException.Message}");
            }
            IsConnected = false;
            throw;
        }
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_socket == null || !_socket.Connected)
        {
            return;
        }

        try
        {
            Log("[CarbonVoiceWebSocketService] Disconnecting...");
            await _socket.DisconnectAsync();
            IsConnected = false;
            Log("[CarbonVoiceWebSocketService] Disconnected successfully");
        }
        catch (Exception ex)
        {
            Log($"[CarbonVoiceWebSocketService] ERROR during disconnect: {ex.GetType().Name} - {ex.Message}");
        }
    }
    
    /// <summary>
    /// Emit an event to the server
    /// </summary>
    public async Task EmitAsync(string eventName, object? data = null)
    {
        if (_socket == null || !_socket.Connected)
        {
            Log($"[CarbonVoiceWebSocketService] Cannot emit event '{eventName}' - not connected");
            return;
        }
        
        try
        {
            await _socket.EmitAsync(eventName, data);
            Log($"[CarbonVoiceWebSocketService] Emitted event: {eventName}");
        }
        catch (Exception ex)
        {
            Log($"[CarbonVoiceWebSocketService] ERROR emitting event '{eventName}': {ex.Message}");
        }
    }
    
    /// <summary>
    /// Subscribe to a specific event. Handlers can be registered before connection and will be applied when the socket connects.
    /// Handlers are persisted across reconnections.
    /// </summary>
    public void On(string eventName, Action<SocketIOResponse> handler)
    {
        // Always store the handler so it persists across reconnections
        _eventHandlers.Add((eventName, handler));
        
        if (_socket != null)
        {
            // Socket is already created, register immediately
            _socket.On(eventName, handler);
            Log($"[CarbonVoiceWebSocketService] Subscribed to event: {eventName}");
        }
        else
        {
            // Socket not created yet, handler will be registered when socket is created
            Log($"[CarbonVoiceWebSocketService] Queued handler for event '{eventName}' (will be registered on connection)");
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _pxtokenSubscription?.Dispose();

        try
        {
            DisconnectAsync().GetAwaiter().GetResult();
        }
        catch
        {
            // Ignore errors during disposal
        }

        _socket?.Dispose();
        _disposed = true;
    }
}


