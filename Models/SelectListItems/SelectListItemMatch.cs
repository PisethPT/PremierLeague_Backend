namespace PremierLeague_Backend.Models.SelectListItems;

public record SelectListItemMatch
(
    int MatchId,
    int HomeClubId,
    int AwayClubId,
    string HomeClubCrest,
    string AwayClubCrest,
    string HomeTheme,
    string AwayTheme,
    string MatchContent,
    string MatchSubContent
);
