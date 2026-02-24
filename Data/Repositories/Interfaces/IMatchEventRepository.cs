using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface IMatchEventRepository
{
    Task<bool> AddMatchEventAsync(MatchEventDto matchEventDto);
    Task<bool> UpdateMatchEventAsync(int matchEventId, MatchEventDto matchEventDto);
    Task<bool> DeleteMatchEventAsync(int matchEventId);
    Task<IEnumerable<MatchEventDuringMatchWeekDto>> GetMatchEventDuringMatchWeekAsync(int? seasonId, int week, int? page = 1, int? competitionId = 1, string? clubIdJson = null, CancellationToken ct = default);
    Task<IEnumerable<MatchEventTypesDto>> GetMatchEventTypesAsync(int? matchId, CancellationToken ct = default);
    Task<IEnumerable<PlayerStatGetPlayersDto>> GetHomePlayersForPlayerStatByClubIdAndMatchIdAsync(int matchId, int clubId, CancellationToken ct = default);
    Task<IEnumerable<PlayerStatGetPlayersDto>> GetAwayPlayersForPlayerStatByClubIdAndMatchIdAsync(int matchId, int clubId, CancellationToken ct = default);
}
