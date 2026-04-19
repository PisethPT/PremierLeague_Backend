namespace PremierLeague_Backend;

public class MatchInfoDto
{
    public int? MatchInfoId { get; set; }
    public int MatchId { get; set; }
    public int Attendance { get; set; }
    public string? Weather { get; set; }
    public string? PitchCondition { get; set; }
    public int? AddedTimeFirstHalf { get; set; }
    public int? AddedTimeSecondHalf { get; set; }
}