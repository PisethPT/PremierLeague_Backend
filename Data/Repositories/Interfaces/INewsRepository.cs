using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface INewsRepository
{
    Task<bool> AddNewsAsync(NewsDto newsDto);
    Task<bool> UpdateNewsAsync(NewsDto newsDto);
    Task<bool> DeleteNewsAsync(int newsId, string? imageUrl);
    Task<NewsDto?> GetNewsByIdAsync(int newsId, CancellationToken ct = default);
    Task<IEnumerable<NewsDetailDto>> GetAllNewsDetailAsync(CancellationToken ct = default);
}
