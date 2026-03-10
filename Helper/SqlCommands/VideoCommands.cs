namespace PremierLeague_Backend.Helper.SqlCommands;

public static class VideoCommands
{
    public static string AddVideoCommand => "PL_AddVideo";
    public static string UpdateVideoCommand => "PL_UpdateVideo";
    public static string DeleteVideoCommand => "PL_DeleteVideo";
    public static string GetVideoByIdCommand => "PL_GetVideoById";
    public static string GetAllVideosCommand => "PL_GetAllVideos";
    public static string FindVideoExistingCommand => "PL_FindVideoExisting";
    public static string CountVideosCommand => "PL_CountVideos";
}
