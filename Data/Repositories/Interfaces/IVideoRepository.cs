using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface IVideoRepository
{
    Task<bool> AddVideoAsync(VideoDto videoDto);
    Task<bool> UpdateVideoAsync(int videoId, VideoDto videoDto);
    Task<bool> DeleteVideoAsync(int videoId );
    Task<bool> FindVideoExisting(VideoDto videoDto, int? videoId = 0);
    Task<VideoDto> GetVideoByIdAsync(int videoId, CancellationToken ct = default);
    Task<IEnumerable<VideoDetailDto>> GetAllVideosAsync(int? page = 1, int? pageSize = 20, CancellationToken ct = default);
    Task<int> GetVideoCountAsync(CancellationToken ct = default);
}
