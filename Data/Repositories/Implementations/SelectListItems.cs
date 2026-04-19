using System.Data.SqlClient;
using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Models.SelectListItems;
using PremierLeague_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using static PremierLeague_Backend.Helper.SqlCommands.SelectListItemCommands;
using PremierLeague_Backend.Helper;

namespace PremierLeague_Backend.Data.Repositories.Implementations;

public class SelectListItems : ISelectListItems
{
    private readonly IExecute execute;

    public SelectListItems(IExecute execute)
    {
        this.execute = execute;
    }

    public async Task<List<SelectListItemClub>> SelectListItemClubAsync(CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = SelectListItemClubCommands;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemClub>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemClub
                    {
                        ClubId = rdr.GetInt32(rdr.GetOrdinal("ClubId")),
                        ClubName = rdr.IsDBNull(rdr.GetOrdinal("ClubName")) ? "" : rdr.GetString(rdr.GetOrdinal("ClubName")),
                        ClubCrest = rdr.IsDBNull(rdr.GetOrdinal("ClubCrest")) ? "" : rdr.GetString(rdr.GetOrdinal("ClubCrest")),
                        ClubTheme = rdr.IsDBNull(rdr.GetOrdinal("ClubTheme")) ? "" : rdr.GetString(rdr.GetOrdinal("ClubTheme"))
                    };
                    SelectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemFormation>> SelectListItemFormationAsync(CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = SelectListItemFormationCommands;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemFormation>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemFormation
                    (
                        rdr.GetInt32(rdr.GetOrdinal("FormationId")),
                        rdr.IsDBNull(rdr.GetOrdinal("PrimaryFormation")) ? "" : rdr.GetString(rdr.GetOrdinal("PrimaryFormation")),
                        rdr.IsDBNull(rdr.GetOrdinal("Tag")) ? "" : rdr.GetString(rdr.GetOrdinal("Tag"))
                    );
                    SelectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemHasImage>> SelectListItemHasImageAsync(string commandText, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = commandText;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemHasImage>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemHasImage
                    {
                        Value = rdr.SafeGetInt("Value"),
                        Label = rdr.SafeGetString("Label"),
                        Img = rdr.SafeGetString("Img")
                    };
                    SelectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemHasSubtitle>> SelectListItemHasSubtitleAsync(string commandText, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = commandText;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemHasSubtitle>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemHasSubtitle
                    {
                        Value = rdr.SafeGetInt("Value"),
                        Label = rdr.SafeGetString("Label"),
                        Subtitle = rdr.SafeGetString("Subtitle")
                    };
                    SelectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemMatch>> SelectListItemMatchesAsync(string commandText, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = commandText;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemMatch>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemMatch
                    (
                        MatchId: rdr.SafeGetInt("MatchId"),
                        HomeClubId: rdr.SafeGetInt("HomeClubId"),
                        AwayClubId: rdr.SafeGetInt("AwayClubId"),
                        HomeClubCrest: rdr.SafeGetString("HomeClubCrest"),
                        AwayClubCrest: rdr.SafeGetString("AwayClubCrest"),
                        HomeTheme: rdr.SafeGetString("HomeClubTheme"),
                        AwayTheme: rdr.SafeGetString("AwayClubTheme"),
                        MatchContent: rdr.SafeGetString("MatchContent"),
                        MatchSubContent: rdr.SafeGetString("MatchSubContent")
                    );
                    SelectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemMatch>> SelectListItemMatchForLineupAsync(CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = SelectListItemMatchForLineupCommands;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemMatch>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemMatch
                    (
                        rdr.GetInt32(rdr.GetOrdinal("MatchId")),
                        rdr.GetInt32(rdr.GetOrdinal("HomeClubId")),
                        rdr.GetInt32(rdr.GetOrdinal("AwayClubId")),
                        rdr.IsDBNull(rdr.GetOrdinal("HomeClubCrest")) ? "" : rdr.GetString(rdr.GetOrdinal("HomeClubCrest")),
                        rdr.IsDBNull(rdr.GetOrdinal("AwayClubCrest")) ? "" : rdr.GetString(rdr.GetOrdinal("AwayClubCrest")),
                        rdr.IsDBNull(rdr.GetOrdinal("HomeClubTheme")) ? "" : rdr.GetString(rdr.GetOrdinal("HomeClubTheme")),
                        rdr.IsDBNull(rdr.GetOrdinal("AwayClubTheme")) ? "" : rdr.GetString(rdr.GetOrdinal("AwayClubTheme")),
                        rdr.IsDBNull(rdr.GetOrdinal("MatchContent")) ? "" : rdr.GetString(rdr.GetOrdinal("MatchContent")),
                        rdr.IsDBNull(rdr.GetOrdinal("MatchSubContent")) ? "" : rdr.GetString(rdr.GetOrdinal("MatchSubContent"))
                    );
                    SelectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemPlayerLineupByClubId>> SelectListItemPlayerLineupByClubIdAsync(int matchId, int clubId, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = SelectListItemPlayerLineupByClubIdCommands;
            cmd.Parameters.AddWithValue("@MatchId", matchId);
            cmd.Parameters.AddWithValue("@ClubId", clubId);
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemPlayerLineupByClubId>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemPlayerLineupByClubId
                    (
                        ClubId: rdr.SafeGetInt("ClubId"),
                        ClubCrest: rdr.SafeGetString("ClubCrest"),
                        ClubTheme: rdr.SafeGetString("ClubTheme"),
                        PlayerId: rdr.SafeGetInt("PlayerId"),
                        FirstName: rdr.SafeGetString("FirstName"),
                        LastName: rdr.SafeGetString("LastName"),
                        Photo: rdr.SafeGetString("Photo"),
                        PlayerNumber: rdr.SafeGetInt("PlayerNumber"),
                        PreferredFoot: rdr.SafeGetString("PreferredFoot"),
                        PositionId: rdr.SafeGetInt("PositionId"),
                        Position: rdr.SafeGetString("Position")
                    );
                    SelectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemReferee>> SelectListItemRefereeAsync(CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = SelectListItemRefereeCommands;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemReferee>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemReferee
                    (
                        rdr.GetInt32(rdr.GetOrdinal("RefereeId")),
                        rdr.IsDBNull(rdr.GetOrdinal("RefereeName")) ? "" : rdr.GetString(rdr.GetOrdinal("RefereeName")),
                        rdr.IsDBNull(rdr.GetOrdinal("DefaultRole")) ? "" : rdr.GetString(rdr.GetOrdinal("DefaultRole")),
                        rdr.IsDBNull(rdr.GetOrdinal("Nationality")) ? "" : rdr.GetString(rdr.GetOrdinal("Nationality"))
                    );
                    SelectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemRefereeBadgeLevel>> SelectListItemRefereeBadgeLevelAsync(CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = SelectListItemRefereeBadgeLevelCommands;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemRefereeBadgeLevel>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemRefereeBadgeLevel
                    (
                        rdr.GetInt32(rdr.GetOrdinal("RefereeBadgeId")),
                        rdr.IsDBNull(rdr.GetOrdinal("BadgeName")) ? "" : rdr.GetString(rdr.GetOrdinal("BadgeName"))
                    );
                    SelectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemRefereeRole>> SelectListItemRefereeRoleAsync(CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = SelectListItemRefereeRoleCommands;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var selectListItem = new List<SelectListItemRefereeRole>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemRefereeRole
                    (
                        rdr.GetInt32(rdr.GetOrdinal("RefereeRoleId")),
                        rdr.IsDBNull(rdr.GetOrdinal("RoleName")) ? "" : rdr.GetString(rdr.GetOrdinal("RoleName"))
                    );
                    selectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return selectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItem>> SelectListItemsAsync(string commandText, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = commandText;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var selectListItem = new List<SelectListItem>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItem
                    {
                        Value = rdr.GetInt32(rdr.GetOrdinal("Value")).ToString(),
                        Text = rdr.IsDBNull(rdr.GetOrdinal("Text")) ? "" : rdr.GetString(rdr.GetOrdinal("Text"))
                    };
                    selectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return selectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItem>> SelectListItemsAsync(string commandText, Dictionary<string, string> sqlParams, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = commandText;
            foreach (var param in sqlParams)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var selectListItem = new List<SelectListItem>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItem
                    {
                        Value = rdr.GetInt32(rdr.GetOrdinal("Value")).ToString(),
                        Text = rdr.IsDBNull(rdr.GetOrdinal("Text")) ? "" : rdr.GetString(rdr.GetOrdinal("Text"))
                    };
                    selectListItem.Add(item);

                } while (await rdr.ReadAsync(ct));
            }
            return selectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SelectListItemSeason>> SelectListItemSeasonAsync(CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = SelectListItemSeasonCommands;
            using var rdr = await execute.ExecuteReaderAsync(cmd);
            var SelectListItem = new List<SelectListItemSeason>();
            if (rdr is not null)
            {
                do
                {
                    var item = new SelectListItemSeason(
                        rdr.GetInt32(rdr.GetOrdinal("SeasonId")),
                        rdr.IsDBNull(rdr.GetOrdinal("SeasonName")) ? "" : rdr.GetString(rdr.GetOrdinal("SeasonName")),
                        rdr.IsDBNull(rdr.GetOrdinal("DataSub")) ? "" : rdr.GetString(rdr.GetOrdinal("DataSub"))
                    );
                    SelectListItem.Add(item);
                } while (await rdr.ReadAsync(ct));
            }
            return SelectListItem;
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
