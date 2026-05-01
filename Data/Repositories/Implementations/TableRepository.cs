using System.Data.SqlClient;
using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Helper;
using PremierLeague_Backend.Models.DTOs;
using PremierLeague_Backend.Services.Interfaces;

namespace PremierLeague_Backend.Data.Repositories.Implementations;

public class TableRepository : ITableRepository
{
    private readonly IExecute execute;


    public TableRepository(IExecute execute)
    {
        this.execute = execute;

    }

    public async Task<IEnumerable<TableDto>> GetTableAsync(int seasonId = 4, CancellationToken ct = default)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandText = "PL_GetTable";
            var rdr = await execute.ExecuteReaderAsync(cmd);
            var values = new List<TableDto>();
            if (rdr is not null)
            {
                do
                {
                    values.Add(new TableDto
                    {
                        Position = rdr.SafeGetInt("Position"),
                        ClubId = rdr.SafeGetInt("ClubId"),
                        ClubName = rdr.SafeGetString("ClubName"),
                        ClubCrest = rdr.SafeGetString("ClubCrest"),
                        Played = rdr.SafeGetInt("Played"),
                        Wins = rdr.SafeGetInt("Wins"),
                        Draws = rdr.SafeGetInt("Draws"),
                        Losses = rdr.SafeGetInt("Losses"),
                        GF = rdr.SafeGetInt("GF"),
                        GA = rdr.SafeGetInt("GA"),
                        GD = rdr.SafeGetInt("GD"),
                        Points = rdr.SafeGetInt("Points"),
                        Form = rdr.SafeGetString("Form"),
                        Next = rdr.SafeGetString("Next"),
                        Qualification = rdr.SafeGetString("Qualification"),
                        PositionStatus = rdr.SafeGetString("PositionStatus"),
                    });
                } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
            }

            return values;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database find match info error: {ex.Message}", ex);
        }
    }

}
