using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface ITableRepository
{
    Task<IEnumerable<TableDto>> GetTableAsync(int seasonId = 4, CancellationToken ct = default);
}
