using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Loudspeaker.ApiClients;

namespace Loudspeaker.ViewModels;

public class MessageHelper : IValueConverter
{
    public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MessageV2 message)
        {
            if (parameter?.ToString() == "Transcript")
            {
                return GetTranscriptText(message);
            }
            else if (parameter?.ToString() == "HasAudio")
            {
                return HasAudioToPlay(message);
            }
        }
        return null;
    }

    public object? ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static string? GetTranscriptText(MessageV2? message)
    {
        if (message?.Text_models == null)
            return null;

        var transcriptModel = message.Text_models.FirstOrDefault(tm => tm.Type == TextModelType.Transcript_with_timecode);
        if (transcriptModel != null && transcriptModel.Timecodes.Any())
        {
            return string.Join( " ", transcriptModel.Timecodes.Select( t => t.T ) );
        }

        transcriptModel = message.Text_models.FirstOrDefault( tm => tm.Type == TextModelType.Transcript );
        return transcriptModel?.Value;
    }

    public static bool HasAudioToPlay(MessageV2? message)
    {
        if (message?.Audio_models == null)
            return false;

        return message.Audio_models.Any(a => !a.Streaming);
    }
}

