using Microsoft.AspNetCore.Mvc.Rendering;
using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Models.ViewModels;

public class MatchEventViewModel
{
    public IEnumerable<MatchEventDuringMatchWeekDto> MatchEventDuringMatchWeekDtos { get; set; }
    public IEnumerable<MatchEventTypesDto> MatchEventTypesDtos { get; set; }
    public MatchEventDto MatchEventDto { get; set; }

    public IEnumerable<SelectListItem> MatchEventTypesSelectListItem { get; set; }
    public IEnumerable<SelectListItem> MatchEventOutcomesSelectListItem { get; set; }
    public IEnumerable<PlayerStatGetPlayersDto> PlayerStatGetPlayersDtos { get; set; }

    public MatchEventViewModel()
    {
        this.MatchEventDuringMatchWeekDtos = new List<MatchEventDuringMatchWeekDto>();
        this.MatchEventTypesDtos = new List<MatchEventTypesDto>();
        this.MatchEventDto = new MatchEventDto();
        this.PlayerStatGetPlayersDtos = new List<PlayerStatGetPlayersDto>();

        this.MatchEventTypesSelectListItem = new List<SelectListItem>();
        this.MatchEventOutcomesSelectListItem = new List<SelectListItem>();
    }
}
