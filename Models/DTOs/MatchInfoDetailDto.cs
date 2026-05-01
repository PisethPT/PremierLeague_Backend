namespace PremierLeague_Backend.Models.DTOs;

public class MatchInfoDetailDto
{
    public int MatchInfoId { get; set; }
    public int MatchId { get; set; }
    public int Attendance { get; set; }
    public string? Weather { get; set; }
    public string? PitchCondition { get; set; }
    public int? AddedTimeFirstHalf { get; set; }
    public int? AddedTimeSecondHalf { get; set; }
    public string HomeClubName { get; set; } = string.Empty;
    public string AwayClubName { get; set; } = string.Empty;
    public string HomeClubCrest { get; set; } = string.Empty;
    public string AwayClubCrest { get; set; } = string.Empty;
    public string HomeClubTheme { get; set; } = string.Empty;
    public string AwayClubTheme { get; set; } = string.Empty;
    public string MatchDate { get; set; } = string.Empty;
    public string MatchTime { get; set; } = string.Empty;
    public string SeasonName { get; set; } = string.Empty;
    public string KickoffStadium { get; set; } = string.Empty;
}
