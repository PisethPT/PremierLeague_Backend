namespace PremierLeague_Backend.Helper.SqlCommands;

public static class LineupCommands
{
    public static string AddMatchLineupCommand => "PL_AddMatchLineup";
    public static string UpdateMatchLineupCommand => "PL_UpdateMatchLineup";
    public static string DeleteMatchLineupCommand => "PL_DeleteMatchLineup";
    public static string IsExistLineupCommand => "PL_IsExistLineup";
    public static string IsMatchCurringKickOffCommand => "PL_IsMatchCurringKickOff";
    public static string IsMatchEndedCommand => "PL_IsMatchEnded";
    public static string GetLineupByMatchIdCommand => "PL_GetLineupByMatchId";
    public static string GetAllLineupsCommand => "PL_GetAllLineups";
    public static string GetHomeClubAndAwayClubByMatchIdCommand => "PL_GetHomeClubAndAwayClubByMatchId";
    public static string GetLineupClubInfoByMatchIdCommand => "PL_GetLineupClubInfoByMatchId";
    public static string GetLineupFormationDetailByMatchIdCommand => "PL_GetLineupFormationByMatchId";
    public static string GetSubstitutionFormationByMatchIdCommand => "PL_GetSubstitutionFormationByMatchId";
    public static string GetLineupFormationLayoutByFormationIdCommand => "PL_GetLineupFormationLayoutByFormationId";
}
