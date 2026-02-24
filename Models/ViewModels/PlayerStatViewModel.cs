using Microsoft.AspNetCore.Mvc.Rendering;
using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Models.ViewModels;

public class PlayerStatViewModel
{
    public int StatCategoryId { get; set; }
    public IEnumerable<PlayerStatMatchListDto> PlayerStatMatchListDtos { get; set; }
    public IEnumerable<PlayerStatGetPlayersDto> PlayerStatGetPlayersDtos { get; set; }
    public IEnumerable<PlayerStatGetStatsByCategoryDto> PlayerStatGetStatsByCategoryDtos { get; set; }
    public IEnumerable<SelectListItem> StatCategoriesSelectListItem { get; set; }
    public PlayerStatDto PlayerStatDto { get; set; }

    public PlayerStatViewModel()
    {
        this.PlayerStatMatchListDtos = new List<PlayerStatMatchListDto>();
        this.PlayerStatGetPlayersDtos = new List<PlayerStatGetPlayersDto>();
        this.PlayerStatGetStatsByCategoryDtos = new List<PlayerStatGetStatsByCategoryDto>();
        this.StatCategoriesSelectListItem = new List<SelectListItem>();

        this.PlayerStatDto = new PlayerStatDto();
    }
}
