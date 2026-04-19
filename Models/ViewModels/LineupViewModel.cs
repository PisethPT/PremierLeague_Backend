using PremierLeague_Backend.Models.DTOs;
using PremierLeague_Backend.Models.SelectListItems;

namespace PremierLeague_Backend.Models.ViewModels;

public class LineupViewModel
{
    public LineupDto LineupDto { get; set; }
    public IEnumerable<LineupDetailDto> LineupDetailDto { get; set; }
    public LineupClubInfoDto LineupClubInfoDto { get; set; }
    public IEnumerable<LineupFormationDetailDto> LineupFormationDetailDto { get; set; }
    public List<SelectListItemMatch> SelectListItemMatchForLineups { get; set; }
    public List<SelectListItemPlayerLineupByClubId> SelectListItemHomeClubPlayer { get; set; }
    public List<SelectListItemPlayerLineupByClubId> SelectListItemAwayClubPlayer { get; set; }
    public List<SelectListItemFormation> SelectListItemFormations { get; set; }
    public LineupViewModel()
    {
        this.LineupDto = new();
        this.LineupDetailDto = new List<LineupDetailDto>();
        this.LineupClubInfoDto = new();
        this.LineupFormationDetailDto = new List<LineupFormationDetailDto>();
        this.SelectListItemMatchForLineups = new();
        this.SelectListItemHomeClubPlayer = new();
        this.SelectListItemAwayClubPlayer = new();
        this.SelectListItemFormations = new();
    }
}
