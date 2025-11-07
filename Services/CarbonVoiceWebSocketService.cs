using System;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.Services;

public class CarbonVoiceWebSocketService : IDisposable
{
    private readonly AuthStateManager _authStateManager;
    private ClientWebSocket? _webSocket;
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _receiveTask;
    private bool _isConnected;
    private bool _disposed;
    private IDisposable? _pxtokenSubscription;

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
        if (_webSocket != null && _webSocket.State == WebSocketState.Open)
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
            Log("[CarbonVoiceWebSocketService] Connecting to wss://ws.carbonvoice.app...");
            
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
            
            // Set the pxtoken header
            _webSocket.Options.SetRequestHeader("pxtoken", pxtoken);
            
            await _webSocket.ConnectAsync(new Uri("wss://ws.carbonvoice.app"), cancellationToken);
            
            IsConnected = true;
            Log("[CarbonVoiceWebSocketService] Successfully connected to WebSocket");
            
            // Start receiving messages
            _receiveTask = Task.Run(() => ReceiveMessagesAsync(_cancellationTokenSource.Token));
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

    private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
    {
        if (_webSocket == null)
        {
            return;
        }

        var buffer = new byte[4096];
        
        try
        {
            while (!cancellationToken.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Log("[CarbonVoiceWebSocketService] WebSocket close message received");
                    await _webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closed by server",
                        cancellationToken);
                    IsConnected = false;
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    
                    // Handle multi-part messages
                    if (!result.EndOfMessage)
                    {
                        var fullMessage = new StringBuilder(message);
                        while (!result.EndOfMessage)
                        {
                            result = await _webSocket.ReceiveAsync(
                                new ArraySegment<byte>(buffer), cancellationToken);
                            fullMessage.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                        }
                        message = fullMessage.ToString();
                    }
                    
                    Log($"[CarbonVoiceWebSocketService] Message received: {message}");
                }
                else if (result.MessageType == WebSocketMessageType.Binary)
                {
                    Log($"[CarbonVoiceWebSocketService] Binary message received ({result.Count} bytes)");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Log("[CarbonVoiceWebSocketService] Receive operation was cancelled");
        }
        catch (WebSocketException ex)
        {
            Log($"[CarbonVoiceWebSocketService] WebSocket error during receive: {ex.WebSocketErrorCode} - {ex.Message}");
            IsConnected = false;
        }
        catch (Exception ex)
        {
            Log($"[CarbonVoiceWebSocketService] ERROR: Exception during receive: {ex.GetType().Name} - {ex.Message}");
            IsConnected = false;
        }
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_webSocket == null || _webSocket.State != WebSocketState.Open)
        {
            return;
        }

        try
        {
            Log("[CarbonVoiceWebSocketService] Disconnecting...");
            _cancellationTokenSource?.Cancel();
            
            await _webSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Client closing",
                cancellationToken);
            
            if (_receiveTask != null)
            {
                await _receiveTask;
            }
            
            IsConnected = false;
            Log("[CarbonVoiceWebSocketService] Disconnected successfully");
        }
        catch (Exception ex)
        {
            Log($"[CarbonVoiceWebSocketService] ERROR during disconnect: {ex.GetType().Name} - {ex.Message}");
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

        _cancellationTokenSource?.Dispose();
        _webSocket?.Dispose();
        _disposed = true;
    }
}

