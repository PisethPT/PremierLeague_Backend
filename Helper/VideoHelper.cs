using System;
using System.Text.RegularExpressions;

namespace PremierLeague_Backend.Helper;

public static class VideoHelper
{
    public static string GetYouTubeId(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var thumbRegex = @"\/vi\/([^\/]{11})";
        var thumbMatch = Regex.Match(input, thumbRegex);
        if (thumbMatch.Success)
            return thumbMatch.Groups[1].Value;

        var videoRegex =
            @"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?|shorts)\/|.*[?&]v=)|youtu\.be\/)([^""&?\/\s]{11})";

        var videoMatch = Regex.Match(input, videoRegex);
        if (videoMatch.Success)
            return videoMatch.Groups[1].Value;

        if (input.Length == 11 && !input.Contains("/"))
            return input;

        return string.Empty;
    }

    public static string GetThumbnailUrl(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "";

        if (input.StartsWith("http://") || input.StartsWith("https://"))
        {
            return input;
        }

        var id = GetYouTubeId(input);
        if (!string.IsNullOrEmpty(id))
        {
            return $"https://i.ytimg.com/vi/{id}/mqdefault.jpg";
        }

        if (input.StartsWith("/"))
        {
            return input;
        }
        return input;
    }
}