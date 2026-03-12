namespace PremierLeague_Backend.Helper.SqlCommands;

public static class LineupCommands
{
    public static string GetAllLineupsCommand => "PL_GetAllLineups";
    public static string GetHomeClubAndAwayClubByMatchIdCommand => "PL_GetHomeClubAndAwayClubByMatchId";
    public static string GetLineupClubInfoByMatchIdCommand => "PL_GetLineupClubInfoByMatchId";
    public static string GetLineupFormationDetailByMatchIdCommand => "PL_GetLineupFormationByMatchId";
    public static string GetSubstitutionFormationByMatchIdCommand => "PL_GetSubstitutionFormationByMatchId";
}
