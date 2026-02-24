namespace PremierLeague_Backend.Models.DTOs;

public class PlayerStatDto
{
    public int? PlayerStatId { get; set; }
    public int MatchId { get; set; }
    public int PlayerId { get; set; }
    public int ClubId { get; set; }
    public int StatId { get; set; }
    public decimal Value { get; set; }
    public decimal? PercentageValue { get; set; }
}