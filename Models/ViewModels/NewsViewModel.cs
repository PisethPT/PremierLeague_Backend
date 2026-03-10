using PremierLeague_Backend.Models.DTOs;
using PremierLeague_Backend.Models.SelectListItems;

namespace PremierLeague_Backend.Models.ViewModels;

public class NewsViewModel
{
    public NewsDto NewsDto { get; set; }
    public IEnumerable<NewsDetailDto> NewsDetailDtos { get; set; }
    public List<SelectListItemHasSubtitle> SelectListItemNewsTag { get; set; }
    public NewsViewModel()
    {
        this.NewsDto = new();
        this.NewsDetailDtos = new List<NewsDetailDto>();
        this.SelectListItemNewsTag = new List<SelectListItemHasSubtitle>();
    }
}
