namespace PremierLeague_Backend;
public class LineupDetailDto
{
    public int MatchId { get; set; }
    public string MatchDate { get; set; } = string.Empty;
    public string KickoffTime { get; set; } = string.Empty;
    public int HomeClubId { get; set; }
    public int AwayClubId { get; set; }
    public string HomeClubName { get; set; } = string.Empty;
    public string AwayClubName { get; set; } = string.Empty;
    public string HomeClubCrest { get; set; } = string.Empty;
    public string AwayClubCrest { get; set; } = string.Empty;
    public string HomeClubTheme { get; set; } = string.Empty;
    public string AwayClubTheme { get; set; } = string.Empty;
    public int HomeClubGoal { get; set; }
    public int AwayClubGoal { get; set; }
    public string KickoffStadium { get; set; } = string.Empty;
    public bool IsGamePlaying { get; set; }
    public string MatchStatus { get; set; } = string.Empty;
}