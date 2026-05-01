namespace PremierLeague_Backend.Helper.SqlCommands;

public static class MatchInfoCommands
{
    public static string AddCommand => "PL_AddMatchInfo";
    public static string DeleteCommand => "PL_DeleteMatchInfo";
    public static string GetExistingCommand => "PL_MatchInfoExisting";
    public static string GetAllCommand => "PL_GetAllMatchInfo";
    public static string FindByIdCommand => "PL_FindMatchInfoById";
    public static string UpdateCommand => "PL_UpdateMatchInfo";
    public static string CountCommand => "PL_CountMatchInfo";
}
