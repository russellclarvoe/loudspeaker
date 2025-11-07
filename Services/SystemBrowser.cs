using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;
using static Loudspeaker.Services.Logger;

namespace Loudspeaker.Services;

public class SystemBrowser : IBrowser
{
    private readonly int _port;
    private readonly string _path;

    public SystemBrowser(int port = 7890, string path = "/")
    {
        _port = port;
        _path = path;
    }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        System.Net.HttpListener? listener = null;
        try
        {
            Log($"[SystemBrowser] Starting HTTP listener on port {_port}");
            listener = new System.Net.HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{_port}{_path}");
            listener.Start();
            Log($"[SystemBrowser] HTTP listener started successfully");

            // Open the browser
            Log($"[SystemBrowser] Opening browser with URL: {options.StartUrl}");
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = options.StartUrl,
                    UseShellExecute = true
                });
                Log("[SystemBrowser] Browser opened successfully");
            }
            catch (Exception ex)
            {
                Log($"[SystemBrowser] ERROR: Failed to open browser: {ex.Message}");
                Log($"[SystemBrowser] Exception type: {ex.GetType().Name}");
                throw;
            }

            // Wait for the callback
            Log("[SystemBrowser] Waiting for OAuth callback...");
            var context = await listener.GetContextAsync();
            var request = context.Request;
            var response = context.Response;

            Log($"[SystemBrowser] Received callback: {request.Url}");
            
            var result = new BrowserResult
            {
                Response = request.Url?.ToString() ?? string.Empty
            };

            // Send a response to the browser
            var responseString = "<html><head><meta http-equiv='refresh' content='0;url=about:blank'></head><body>You can close this window.</body></html>";
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/html";
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
            response.Close();

            Log("[SystemBrowser] Sent response to browser, closing listener");
            listener.Stop();
            listener = null;

            return result;
        }
        catch (HttpListenerException ex)
        {
            Log($"[SystemBrowser] ERROR: HTTP listener exception: {ex.Message}");
            Log($"[SystemBrowser] Error code: {ex.ErrorCode}");
            if (ex.ErrorCode == 5) // Access denied
            {
                Log("[SystemBrowser] ERROR: Access denied. You may need to run the app as administrator or reserve the URL.");
            }
            throw;
        }
        catch (Exception ex)
        {
            Log($"[SystemBrowser] ERROR: Exception in browser flow: {ex.GetType().Name}");
            Log($"[SystemBrowser] Exception message: {ex.Message}");
            Log($"[SystemBrowser] Stack trace: {ex.StackTrace}");
            throw;
        }
        finally
        {
            if (listener != null && listener.IsListening)
            {
                try
                {
                    listener.Stop();
                    Log("[SystemBrowser] Listener stopped in finally block");
                }
                catch (Exception ex)
                {
                    Log($"[SystemBrowser] Error stopping listener: {ex.Message}");
                }
            }
        }
    }
}

