namespace PremierLeague_Backend.Models.DTOs;

public class LineupFormationLayout
{
    public int Row { get; set; }
    public int PositionId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string FormationName { get; set; } = string.Empty;
}