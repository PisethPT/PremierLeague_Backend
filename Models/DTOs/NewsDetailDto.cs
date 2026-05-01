namespace PremierLeague_Backend.Models.DTOs
{
    public class NewsDetailDto
    {
        public int NewsId { get; set; }
        public int NewsTagId { get; set; }
        public int VideoCategoryId { get; set; }
        public string NewsTagName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ReferenceUrl { get; set; } = string.Empty;
        public string VideoReferenceUrl { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string PublishedDate { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public int ClubId { get; set; }
        public int MatchId { get; set; }
        public bool IsActive { get; set; }
    }
}