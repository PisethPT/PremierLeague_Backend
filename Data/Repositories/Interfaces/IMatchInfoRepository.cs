namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface IMatchInfoRepository
{
    Task<bool> AddAsync(MatchInfoDto matchInfoDto);
    Task<bool> UpdateAsync(MatchInfoDto matchInfoDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsExistAsync(int id);
    Task<MatchInfoDto> GetByIdAsync(int id);
    Task<IEnumerable<MatchInfoDto>> GetAllAsync();
}