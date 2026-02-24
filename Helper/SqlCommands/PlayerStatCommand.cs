namespace PremierLeague_Backend.Helper.SqlCommands;

public static class PlayerStatCommand
{
    public static string GetPlayerStatMatchListCommand => "PL_GetPlayerStatMatchList";
    public static string GetStatByIdForPlayerStatCommand => "PL_GetStatByIdForPlayerStat";
    public static string GetHomePlayersForPlayerStatByClubIdAndMatchIdCommand => "PL_GetHomePlayersForPlayerStatByClubIdAndMatchId";
    public static string GetAwayPlayersForPlayerStatByClubIdAndMatchIdCommand => "PL_GetAwayPlayersForPlayerStatByClubIdAndMatchId";
}
