using PremierLeague_Backend.Models.DTOs;
using PremierLeague_Backend.Models.SelectListItems;

namespace PremierLeague_Backend.Models.ViewModels;

public class VideoViewModel
{
    public IEnumerable<VideoDetailDto> VideoDetailDtos { get; set; }
    public VideoDto VideoDto { get; set; }
    public IEnumerable<SelectListItemHasSubtitle> SelectListItemVideoCategory { get; set; }
    public VideoViewModel()
    {
        this.VideoDetailDtos = new List<VideoDetailDto>();
        this.VideoDto = new VideoDto();
        this.SelectListItemVideoCategory = new List<SelectListItemHasSubtitle>();
    }
}
