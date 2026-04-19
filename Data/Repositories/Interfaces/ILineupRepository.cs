using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface ILineupRepository
{
    Task<bool> AddLineupAsync(LineupDto lineupDto);
    Task<bool> UpdateLineupAsync(int matchId, LineupDto lineupDto);
    Task<bool> DeleteLineupAsync(int matchId);
    Task<bool> IsExistLineupAsync(int matchId);
    Task<bool> IsMatchCurringKickOffAsync(int matchId);
    Task<bool> IsMatchEndedAsync(int matchId);
    Task<LineupDto> GetLineupByMatchIdAsync(int matchId, CancellationToken ct = default);
    Task<IEnumerable<LineupDetailDto>> GetAllLineupsAsync(int? page = 1, int? pageSize = 20, CancellationToken ct = default);
    Task<LineupClubInfoDto> GetLineupClubInfoByMatchIdAsync(int matchId, CancellationToken ct = default);
    Task<IEnumerable<LineupFormationDetailDto>> GetLineupFormationByMatchIdAsync(int matchId, CancellationToken ct = default);
    Task<IEnumerable<LineupFormationDetailDto>> GetSubstitutionFormationByMatchIdAsync(int matchId, CancellationToken ct = default);
    Task<(int, int)> GetHomeClubAndAwayClubByMatchIdAsync(int matchId);
    Task<IEnumerable<LineupFormationLayout>> GetLineupFormationLayoutByFormationIdAsync(int formationId = 1, CancellationToken ct = default);
}
