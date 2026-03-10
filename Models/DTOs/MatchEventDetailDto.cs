namespace PremierLeague_Backend.Models.DTOs;
public class MatchEventDetailDto
{
    public int MatchEventId { get; set; }
    public int MatchId { get; set; }
    public int ClubId { get; set; }
    public int PlayerId { get; set; }
    public int EventTypeId { get; set; }
    public string? MatchEventTypeName { get; set; }
    public int OutcomeId { get; set; }
    public string? OutcomeName { get; set; }
    public string? ClubName { get; set; }
    public string? ClubCrest { get; set; }
    public string? ClubTheme { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Position { get; set; }
    public int PlayerNumber { get; set; }
    public string? Photo { get; set; }
    public string? Minute { get; set; }
    public bool IsPenalty { get; set; }
    public bool IsOwnGoal { get; set; }
    public bool IsHomeClub { get; set; }
}