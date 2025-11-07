using System;
using System.IO;
using System.Threading;

namespace Loudspeaker.Services;

public static class Logger
{
    private static readonly string LogFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Loudspeaker",
        "logs",
        $"loudspeaker-{DateTime.Now:yyyy-MM-dd}.log"
    );

    private static readonly object _lock = new object();

    static Logger()
    {
        // Ensure log directory exists
        var logDir = Path.GetDirectoryName(LogFilePath);
        if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
        {
            Directory.CreateDirectory(logDir);
        }
    }

    public static void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var logMessage = $"[{timestamp}] {message}";

        // Write to console
        Console.WriteLine(logMessage);

        // Write to file
        lock (_lock)
        {
            try
            {
                File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
            }
            catch
            {
                // Silently fail if we can't write to file
            }
        }
    }

    public static string GetLogFilePath() => LogFilePath;
}

