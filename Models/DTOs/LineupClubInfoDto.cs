namespace PremierLeague_Backend;

public class LineupClubInfoDto
{
    public int MatchId { get; set; }
    public int HomeClubId { get; set; }
    public int AwayClubId { get; set; }
    public string HomeClubName { get; set; } = string.Empty;
    public string AwayClubName { get; set; } = string.Empty;
    public string HomeClubManager { get; set; } = string.Empty;
    public string AwayClubManager { get; set; } = string.Empty;
    public string HomeClubCrest { get; set; } = string.Empty;
    public string AwayClubCrest { get; set; } = string.Empty;
    public string HomeClubTheme { get; set; } = string.Empty;
    public string AwayClubTheme { get; set; } = string.Empty;
    public int HomeClubFormationId { get; set; }
    public int AwayClubFormationId { get; set; }
    public string HomeClubFormation { get; set; } = string.Empty;
    public string AwayClubFormation { get; set; } = string.Empty;
}