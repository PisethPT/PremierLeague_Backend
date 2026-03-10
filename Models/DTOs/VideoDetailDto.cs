namespace PremierLeague_Backend.Models.DTOs;

public class VideoDetailDto
{
    public int? VideoId { get; set; }
    public string Title { get; set; }
    public string Channel { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }
    public string VideoUrl { get; set; }
    public string VideoCategory { get; set; }
    public string DurationSeconds { get; set; }
    public string Publisher { get; set; }
    public string PublishedDate { get; set; }
    public string ExpiryDate { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
}
