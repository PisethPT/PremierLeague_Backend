namespace PremierLeague_Backend.Helper.SqlCommands;

public static class MatchEventCommands
{
    public static string AddMatchEventCommand => "PL_AddMatchEvent";
    public static string UpdateMatchEventCommand => "PL_UpdateMatchEvent";
    public static string DeleteMatchEventCommand => "PL_DeleteMatchEvent";
    public static string GetMatchDuringMatchWeekCommand => "PL_GetMatchDuringMatchWeek";
    public static string GetMatchEventTypesCommand => "PL_GetMatchEventTypes";
    public static string GetHomePlayersForPlayerStatByClubIdAndMatchIdCommand => "PL_GetHomePlayersForPlayerStatByClubIdAndMatchId";
    public static string GetAwayPlayersForPlayerStatByClubIdAndMatchIdCommand => "PL_GetAwayPlayersForPlayerStatByClubIdAndMatchId";
}
