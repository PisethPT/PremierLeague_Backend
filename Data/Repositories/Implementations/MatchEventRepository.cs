using System.Data.SqlClient;
using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Helper;
using PremierLeague_Backend.Models.DTOs;
using PremierLeague_Backend.Services.Interfaces;
using static PremierLeague_Backend.Helper.SqlCommands.MatchEventCommands;

namespace PremierLeague_Backend.Data.Repositories.Implementations;

public class MatchEventRepository : IMatchEventRepository
{
    private readonly IExecute execute;

    public MatchEventRepository(IExecute execute)
    {
        this.execute = execute;
    }

    public async Task<bool> AddMatchEventAsync(MatchEventDto matchEventDto)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = AddMatchEventCommand;
            cmd.Parameters.AddWithValue("@MatchId", matchEventDto.MatchId);
            cmd.Parameters.AddWithValue("@PlayerId", matchEventDto.PlayerId);
            cmd.Parameters.AddWithValue("@ClubId", matchEventDto.ClubId);
            cmd.Parameters.AddWithValue("@RelatedEventId", matchEventDto.RelatedEventId);
            cmd.Parameters.AddWithValue("@EventTypeId", matchEventDto.EventTypeId);
            cmd.Parameters.AddWithValue("@OutcomeId", matchEventDto.OutcomeId);
            cmd.Parameters.AddWithValue("@Minute", matchEventDto.Minute);
            cmd.Parameters.AddWithValue("@IsPenalty", matchEventDto.IsPenalty);
            cmd.Parameters.AddWithValue("@IsOwnGoal", matchEventDto.IsOwnGoal);
            cmd.Parameters.AddWithValue("@IsInSideBox", matchEventDto.IsInsideBox);
            cmd.Parameters.AddWithValue("@IsBigChance", matchEventDto.IsBigChance);
            cmd.Parameters.AddWithValue("@IsWoodwork", matchEventDto.IsWoodwork);

            return await execute.ExecuteScalarAsync<bool>(cmd) ? false : true;
        }catch(SqlException ex)
        {
            throw new Exception($"Database error while adding match event: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteMatchEventAsync(int matchEventId)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = DeleteMatchEventCommand;
            cmd.Parameters.AddWithValue("@MatchEventId", matchEventId);
            return await execute.ExecuteScalarAsync<bool>(cmd) ? false : true;
        }catch(SqlException ex)
        {
            throw new Exception($"Database error while deleting match event: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<PlayerStatGetPlayersDto>> GetAwayPlayersForPlayerStatByClubIdAndMatchIdAsync(int matchId, int clubId, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = GetAwayPlayersForPlayerStatByClubIdAndMatchIdCommand;
            cmd.Parameters.AddWithValue("@MatchId", matchId);
            cmd.Parameters.AddWithValue("@ClubId", clubId);
            var rdr = await execute.ExecuteReaderAsync(cmd);
            var players = new List<PlayerStatGetPlayersDto>();
            if (rdr is not null)
            {
                do
                {
                    players.Add(new PlayerStatGetPlayersDto(
                        MatchId: rdr.SafeGetInt("MatchId"),
                        ClubId: rdr.SafeGetInt("ClubId"),
                        PlayerId: rdr.SafeGetInt("PlayerId"),
                        ClubName: rdr.SafeGetString("ClubName"),
                        ClubCrest: rdr.SafeGetString("ClubCrest"),
                        ClubTheme: rdr.SafeGetString("ClubTheme"),
                        FirstName: rdr.SafeGetString("FirstName"),
                        LastName: rdr.SafeGetString("LastName"),
                        Position: rdr.SafeGetString("Position"),
                        PlayerNumber: rdr.SafeGetInt("PlayerNumber"),
                        Photo: rdr.SafeGetString("Photo")
                    ));
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
            }
            return players;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<PlayerStatGetPlayersDto>> GetHomePlayersForPlayerStatByClubIdAndMatchIdAsync(int matchId, int clubId, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = GetHomePlayersForPlayerStatByClubIdAndMatchIdCommand;
            cmd.Parameters.AddWithValue("@MatchId", matchId);
            cmd.Parameters.AddWithValue("@ClubId", clubId);
            var rdr = await execute.ExecuteReaderAsync(cmd);
            var players = new List<PlayerStatGetPlayersDto>();
            if (rdr is not null)
            {
                do
                {
                    players.Add(new PlayerStatGetPlayersDto(
                        MatchId: rdr.SafeGetInt("MatchId"),
                        ClubId: rdr.SafeGetInt("ClubId"),
                        PlayerId: rdr.SafeGetInt("PlayerId"),
                        ClubName: rdr.SafeGetString("ClubName"),
                        ClubCrest: rdr.SafeGetString("ClubCrest"),
                        ClubTheme: rdr.SafeGetString("ClubTheme"),
                        FirstName: rdr.SafeGetString("FirstName"),
                        LastName: rdr.SafeGetString("LastName"),
                        Position: rdr.SafeGetString("Position"),
                        PlayerNumber: rdr.SafeGetInt("PlayerNumber"),
                        Photo: rdr.SafeGetString("Photo")
                    ));
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
            }
            return players;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<MatchEventDuringMatchWeekDto>> GetMatchEventDuringMatchWeekAsync(int? seasonId, int week, int? page = 1, int? competitionId = 1, string? clubIdJson = null, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = GetMatchDuringMatchWeekCommand;
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@SeasonId", seasonId);
            cmd.Parameters.AddWithValue("@Week", week);
            cmd.Parameters.AddWithValue("@CompetitionId", competitionId);
            cmd.Parameters.AddWithValue("@ClubIdJson", clubIdJson ?? (object)DBNull.Value);
            var rdr = await execute.ExecuteReaderAsync(cmd);
            var matchEventDuringMatchWeek = new List<MatchEventDuringMatchWeekDto>();
            if (rdr is not null)
            {
                do
                {
                    matchEventDuringMatchWeek.Add(new MatchEventDuringMatchWeekDto
                    (
                        MatchId: rdr.SafeGetInt("MatchId"),
                        MatchDate: rdr.SafeGetString("MatchDate"),
                        MatchTime: rdr.SafeGetString("MatchTime"),
                        SeasonName: rdr.SafeGetString("SeasonName"),
                        MatchWeek: rdr.SafeGetInt("HomeClubId"),
                        HomeClubId: rdr.SafeGetInt("HomeClubId"),
                        HomeClubName: rdr.SafeGetString("HomeClubName"),
                        HomeClubCrest: rdr.SafeGetString("HomeClubCrest"),
                        HomeClubTheme: rdr.SafeGetString("HomeClubTheme"),
                        HomeClubGoal: rdr.SafeGetInt("HomeClubGoal"),
                        AwayClubId: rdr.SafeGetInt("AwayClubId"),
                        AwayClubName: rdr.SafeGetString("AwayClubName"),
                        AwayClubCrest: rdr.SafeGetString("AwayClubCrest"),
                        AwayClubTheme: rdr.SafeGetString("AwayClubTheme"),
                        AwayClubGoal: rdr.SafeGetInt("AwayClubGoal"),
                        KickoffStadium: rdr.SafeGetString("KickoffStadium"),
                        IsGamePlaying: rdr.SafeGetBoolean("IsGamePlaying"),
                        MatchStatus: rdr.SafeGetString("MatchStatus")
                    ));
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
            }
            return matchEventDuringMatchWeek;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while fetching match during match week: {ex.Message}", ex);
        }
    }

    public async Task<MatchEventDto> FindMatchEventByIdAsync(int matchEventId, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = FindMatchEventByIdCommand;
            cmd.Parameters.AddWithValue("@MatchEventId", matchEventId);
            var rdr = await execute.ExecuteReaderAsync(cmd);    
            var matchEvent = new MatchEventDto();
            if (rdr is not null)
            {
                do
                {
                    matchEvent = new MatchEventDto{
                        MatchEventId = rdr.SafeGetInt("MatchEventId"),
                        MatchId = rdr.SafeGetInt("MatchId"),
                        PlayerId = rdr.SafeGetInt("PlayerId"),
                        ClubId = rdr.SafeGetInt("ClubId"),
                        RelatedEventId = rdr.SafeGetInt("RelatedEventId"),
                        EventTypeId = rdr.SafeGetInt("EventTypeId"),
                        OutcomeId = rdr.SafeGetInt("OutcomeId"),
                        Minute = rdr.SafeGetString("Minute"),
                        IsPenalty = rdr.SafeGetBoolean("IsPenalty"),
                        IsOwnGoal = rdr.SafeGetBoolean("IsOwnGoal"),
                        IsInsideBox = rdr.SafeGetBoolean("IsInsideBox"),
                        IsBigChance = rdr.SafeGetBoolean("IsBigChance"),
                        IsWoodwork = rdr.SafeGetBoolean("IsWoodwork")
                    };
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
            }
            return matchEvent;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while fetching match events by match id: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<MatchEventTypesDto>> GetMatchEventTypesAsync(int? matchId, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = GetMatchEventTypesCommand;
            cmd.Parameters.AddWithValue("@MatchId", matchId);
            var rdr = await execute.ExecuteReaderAsync(cmd);
            var matchEventTypes = new List<MatchEventTypesDto>();
            if (rdr is not null)
            {
                do
                {
                    matchEventTypes.Add(new MatchEventTypesDto
                    {
                        EventTypeId = rdr.SafeGetInt("EventTypeId"),
                        EventTypeName = rdr.SafeGetString("EventTypeName")
                    });
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
            }
            return matchEventTypes;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while fetching match event types: {ex.Message}", ex);
        }
    }

    public async Task<bool> UpdateMatchEventAsync(int matchEventId, MatchEventDto matchEventDto)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = UpdateMatchEventCommand;
            cmd.Parameters.AddWithValue("@MatchEventId", matchEventId);
            cmd.Parameters.AddWithValue("@PlayerId", matchEventDto.PlayerId);
            cmd.Parameters.AddWithValue("@RelatedEventId", matchEventDto.RelatedEventId);
            cmd.Parameters.AddWithValue("@EventTypeId", matchEventDto.EventTypeId);
            cmd.Parameters.AddWithValue("@OutcomeId", matchEventDto.OutcomeId);
            cmd.Parameters.AddWithValue("@Minute", matchEventDto.Minute);
            cmd.Parameters.AddWithValue("@IsPenalty", matchEventDto.IsPenalty);
            cmd.Parameters.AddWithValue("@IsOwnGoal", matchEventDto.IsOwnGoal);
            cmd.Parameters.AddWithValue("@IsInsideBox", matchEventDto.IsInsideBox);
            cmd.Parameters.AddWithValue("@IsBigChance", matchEventDto.IsBigChance);
            cmd.Parameters.AddWithValue("@IsWoodwork", matchEventDto.IsWoodwork);
            return await execute.ExecuteScalarAsync<bool>(cmd) ? false : true;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while updating match event: {ex.Message}", ex);
        }
    }

    public async Task<bool> FindExistingMatchEventPerClubIdAndMatchIdAsync(int clubId, int matchId, int playerId, string minute, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = FindExistingMatchEventPerClubIdAndMatchIdCommand;
            cmd.Parameters.AddWithValue("@ClubId", clubId);
            cmd.Parameters.AddWithValue("@MatchId", matchId);
            cmd.Parameters.AddWithValue("@PlayerId", playerId);
            cmd.Parameters.AddWithValue("@Minute", minute);
            return await execute.ExecuteScalarAsync<bool>(cmd) ? false : true;
        }catch (SqlException ex)
        {
            throw new Exception($"Database error while fetching match event: {ex.Message}", ex);
        }
    }

    public async Task<bool> FindExistsMatchEventByMatchEventIdAsync(MatchEventDto matchEventDto, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = FindExistsMatchEventByMatchEventIdCommand;
            cmd.Parameters.AddWithValue("@MatchEventId", matchEventDto.MatchEventId);
            cmd.Parameters.AddWithValue("@MatchId", matchEventDto.MatchId);
            cmd.Parameters.AddWithValue("@ClubId", matchEventDto.ClubId);
            cmd.Parameters.AddWithValue("@PlayerId", matchEventDto.PlayerId);
            cmd.Parameters.AddWithValue("@Minute", matchEventDto.Minute);
            return await execute.ExecuteScalarAsync<bool>(cmd) ? false : true;
        }catch (SqlException ex)
        {
            throw new Exception($"Database error while fetching match event: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<MatchEventTypesDto>> GetMatchEventsByMatchIdAsync(int matchId, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = GetMatchEventsByMatchIdCommand;
            cmd.Parameters.AddWithValue("@MatchId", matchId);
            var rdr = await execute.ExecuteReaderAsync(cmd);
            var matchEventTypes = new List<MatchEventTypesDto>();
            if (rdr is not null)
            {
                do
                {
                    matchEventTypes.Add(new MatchEventTypesDto
                    {
                        EventTypeId = rdr.SafeGetInt("EventTypeId"),
                        EventTypeName = rdr.SafeGetString("EventTypeName")
                    });
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
            }
            return matchEventTypes;
        }catch(SqlException ex)
        {
            throw new Exception($"Database error while fetching match events by match id: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<MatchEventDetailDto>> GetMatchEventDetailByMatchIdAsync(int matchId, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = GetMatchEventsByMatchIdCommand;
            cmd.Parameters.AddWithValue("@MatchId", matchId);
            var rdr = await execute.ExecuteReaderAsync(cmd);
            var matchEventDetails = new List<MatchEventDetailDto>();
            if (rdr is not null)
            {
                do
                {
                    matchEventDetails.Add(new MatchEventDetailDto
                    {
                        MatchEventId = rdr.SafeGetInt("MatchEventId"),
                        MatchId = rdr.SafeGetInt("MatchId"),
                        ClubId = rdr.SafeGetInt("ClubId"),
                        PlayerId = rdr.SafeGetInt("PlayerId"),
                        EventTypeId = rdr.SafeGetInt("EventTypeId"),
                        OutcomeId = rdr.SafeGetInt("OutcomeId"),
                        MatchEventTypeName = rdr.SafeGetString("MatchEventTypeName"),
                        OutcomeName = rdr.SafeGetString("OutcomeName"),
                        ClubName = rdr.SafeGetString("ClubName"),
                        ClubCrest = rdr.SafeGetString("ClubCrest"),
                        ClubTheme = rdr.SafeGetString("ClubTheme"),
                        FirstName = rdr.SafeGetString("FirstName"),
                        LastName = rdr.SafeGetString("LastName"),
                        Position = rdr.SafeGetString("Position"),
                        PlayerNumber = rdr.SafeGetInt("PlayerNumber"),
                        Photo = rdr.SafeGetString("Photo"),
                        Minute = rdr.SafeGetString("Minute"),
                        IsPenalty = rdr.SafeGetBoolean("IsPenalty"),
                        IsOwnGoal = rdr.SafeGetBoolean("IsOwnGoal"),
                        IsHomeClub = rdr.SafeGetBoolean("IsHomeClub")
                    });
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
            }
            return matchEventDetails;
        }catch(SqlException ex)
        {
            throw new Exception($"Database error while fetching match event details by match id: {ex.Message}", ex);
        }
    }
}
