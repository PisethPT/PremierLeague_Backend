namespace PremierLeague_Backend.Helper;
public static class VideoHelper
{
    public static string GetYouTubeId(string url)
    {
        if (string.IsNullOrEmpty(url)) return "";
        // Handles https://www.youtube.com/watch?v=XVp-ZjKB2mk
        var uri = new Uri(url);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        return query["v"] ?? url.Split('/').Last();
    }

    public static string GetThumbnailUrl(string url)
    {
        string id = GetYouTubeId(url);
        // Returns the high-definition thumbnail
        return $"https://img.youtube.com/vi/{id}/maxresdefault.jpg";
    }
}