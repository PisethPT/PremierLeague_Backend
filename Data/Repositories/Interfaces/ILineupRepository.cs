using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface ILineupRepository
{
    Task<bool> CreateLineupAsync(LineupDto lineupDto);
    Task<IEnumerable<LineupDetailDto>> GetAllLineupsAsync(int? page = 1, int? pageSize = 20, CancellationToken ct = default);
    Task<LineupClubInfoDto> GetLineupClubInfoByMatchIdAsync(int matchId, CancellationToken ct = default);
    Task<IEnumerable<LineupFormationDetailDto>> GetLineupFormationByMatchIdAsync(int matchId, CancellationToken ct = default);
    Task<IEnumerable<LineupFormationDetailDto>> GetSubstitutionFormationByMatchIdAsync(int matchId, CancellationToken ct = default);
    Task<(int, int)> GetHomeClubAndAwayClubByMatchIdAsync(int matchId);
}
