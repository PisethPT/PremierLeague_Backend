using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface IMatchInfoRepository
{
    Task<bool> AddAsync(MatchInfoDto matchInfoDto);
    Task<bool> UpdateAsync(MatchInfoDto matchInfoDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsExistAsync(int id);
    Task<MatchInfoDto> FindByIdAsync(int id);
    Task<IEnumerable<MatchInfoDetailDto>> GetAllAsync(int page = 1, CancellationToken ct = default);
    Task<int> GetCountAsync(CancellationToken ct = default);
}