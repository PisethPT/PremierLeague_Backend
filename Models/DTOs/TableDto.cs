namespace PremierLeague_Backend.Models.DTOs;

public class TableDto
{
    public int Position { get; set; }
    public int ClubId { get; set; }
    public string ClubName { get; set; } = string.Empty;
    public string ClubCrest { get; set; } = string.Empty;
    public int Played { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int GF { get; set; }
    public int GA { get; set; }
    public int GD { get; set; }
    public int Points { get; set; }
    public string Form { get; set; } = string.Empty;
    public string Next { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string PositionStatus { get; set; } = string.Empty;
}
