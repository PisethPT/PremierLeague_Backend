using PremierLeague_Backend.Models.DTOs;
using PremierLeague_Backend.Models.SelectListItems;

namespace PremierLeague_Backend.Models.ViewModels;

public class MatchInfoViewModel
{
    public MatchInfoDto matchInfoDto { get; set; }
    public IEnumerable<MatchInfoDetailDto> matchInfos { get; set; }
    public IEnumerable<SelectListItemMatch> SelectListItemMatches { get; set; }

    public MatchInfoViewModel()
    {
        matchInfoDto = new MatchInfoDto();
        matchInfos = new List<MatchInfoDetailDto>();
        SelectListItemMatches = new List<SelectListItemMatch>();
    }
}
