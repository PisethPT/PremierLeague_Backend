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

    public Task<bool> DeleteMatchEventAsync(int matchEventId)
    {
        throw new NotImplementedException();
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
                        MatchEventId = rdr.SafeGetInt("MatchEventId"),
                        MatchEventTypeName = rdr.SafeGetString("MatchEventTypeName")
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

    public Task<bool> UpdateMatchEventAsync(int matchEventId, MatchEventDto matchEventDto)
    {
        throw new NotImplementedException();
    }
}
