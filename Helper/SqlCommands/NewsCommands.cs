namespace PremierLeague_Backend.Helper.SqlCommands;

public static class NewsCommands
{
    public static string AddNewsCommand => "PL_AddNews";
    public static string UpdateNewsCommand => "PL_UpdateNews";
    public static string DeleteNewsCommand => "PL_DeleteNews";
    public static string GetNewsByIdCommand => "PL_GetNewsById";
    public static string GetAllNewsDetailCommand => "PL_GetAllNewsDetail";
}