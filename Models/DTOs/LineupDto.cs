namespace PremierLeague_Backend.Models.DTOs;

public class LineupDto
{
    public int MatchId { get; set; }
    public int HomeClubFormationId { get; set; }
    public int AwayClubFormationId { get; set; }
    public List<LineupClubDto> HomeClubLineup { get; set; } = new();
    public List<LineupClubDto> AwayClubLineup { get; set; } = new();
}
