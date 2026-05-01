using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface IMatchEventRepository
{
    Task<bool> AddMatchEventAsync(MatchEventDto matchEventDto);
    Task<bool> UpdateMatchEventAsync(int matchEventId, MatchEventDto matchEventDto);
    Task<bool> DeleteMatchEventAsync(int matchEventId);
    Task<IEnumerable<MatchEventDetailDto>> GetMatchEventDetailByMatchIdAsync(int matchId, CancellationToken ct = default);
    Task<IEnumerable<MatchEventTypesDto>> GetMatchEventsByMatchIdAsync(int matchId, CancellationToken ct = default);
    Task<bool> FindExistingMatchEventPerClubIdAndMatchIdAsync(int clubId, int matchId, int playerId, string minute, CancellationToken ct = default);
    Task<bool> FindExistsMatchEventByMatchEventIdAsync(MatchEventDto matchEventDto, CancellationToken ct = default);
    Task<MatchEventDto> FindMatchEventByIdAsync(int matchEventId, CancellationToken ct = default);
    Task<IEnumerable<MatchEventDuringMatchWeekDto>> GetMatchEventDuringMatchWeekAsync(int? seasonId, int week, int? page = 1, int? competitionId = 1, string? clubIdJson = null, CancellationToken ct = default);
    Task<IEnumerable<MatchEventTypesDto>> GetMatchEventTypesAsync(CancellationToken ct = default);
    Task<IEnumerable<PlayerStatGetPlayersDto>> GetHomePlayersForPlayerStatByClubIdAndMatchIdAsync(int matchId, int clubId, CancellationToken ct = default);
    Task<IEnumerable<PlayerStatGetPlayersDto>> GetAwayPlayersForPlayerStatByClubIdAndMatchIdAsync(int matchId, int clubId, CancellationToken ct = default);
}
