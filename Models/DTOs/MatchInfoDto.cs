using System.ComponentModel.DataAnnotations;

namespace PremierLeague_Backend;

public class MatchInfoDto
{
    public int? MatchInfoId { get; set; }
    [Required(ErrorMessage = "Match is required.")]
    public int MatchId { get; set; }
    public int Attendance { get; set; }
    public string? Weather { get; set; }
    public string? PitchCondition { get; set; }
    public int? AddedTimeFirstHalf { get; set; }
    public int? AddedTimeSecondHalf { get; set; }
}