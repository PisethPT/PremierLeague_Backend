using System.ComponentModel.DataAnnotations;

namespace PremierLeague_Backend.Models.DTOs;

public class MatchEventDto
{
    public int? MatchEventId { get; set; }
    [Required(ErrorMessage = "Match is required.")]
    public int MatchId { get; set; }
    [Required(ErrorMessage = "Please select a player.")]
    public int PlayerId { get; set; }
    [Required(ErrorMessage = "Club is required.")]
    public int ClubId { get; set; }
    public int? RelatedEventId { get; set; }
    [Required(ErrorMessage = "Please select an event type for this match event.")]
    public int EventTypeId { get; set; }
    [Required(ErrorMessage = "Please select an outcome for this event type.")]
    public int OutcomeId { get; set; }
    [Required(ErrorMessage = "Minute is required.")]
    public string? Minute { get; set; }
    public bool IsPenalty { get; set; } = false;
    public bool IsOwnGoal { get; set; } = false;
    public bool IsInsideBox { get; set; } = false;
    public bool IsBigChance { get; set; } = false;
    public bool IsWoodwork { get; set; } = false;
}
