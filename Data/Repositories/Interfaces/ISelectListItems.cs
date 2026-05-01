using PremierLeague_Backend.Models.SelectListItems;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace PremierLeague_Backend.Data.Repositories.Interfaces;

public interface ISelectListItems
{
    Task<List<SelectListItem>> SelectListItemsAsync(string commandText, CancellationToken ct = default);
    Task<List<SelectListItem>> SelectListItemsAsync(string commandText, Dictionary<string, string> sqlParams, CancellationToken ct = default);
    Task<List<SelectListItemClub>> SelectListItemClubAsync(CancellationToken ct = default);
    Task<List<SelectListItemSeason>> SelectListItemSeasonAsync(CancellationToken ct = default);
    Task<List<SelectListItemPlayerLineupByClubId>> SelectListItemPlayerLineupByClubIdAsync(int matchId, int clubId, CancellationToken ct = default);
    Task<List<SelectListItemReferee>> SelectListItemRefereeAsync(CancellationToken ct = default);
    Task<List<SelectListItemRefereeRole>> SelectListItemRefereeRoleAsync(CancellationToken ct = default);
    Task<List<SelectListItemRefereeBadgeLevel>> SelectListItemRefereeBadgeLevelAsync(CancellationToken ct = default);
    Task<List<SelectListItemMatch>> SelectListItemMatchForLineupAsync(bool isEdit, CancellationToken ct = default);
    Task<List<SelectListItemFormation>> SelectListItemFormationAsync(CancellationToken ct = default);
    Task<List<SelectListItemHasSubtitle>> SelectListItemHasSubtitleAsync(string commandText, CancellationToken ct = default);
    Task<List<SelectListItemHasImage>> SelectListItemHasImageAsync(string commandText, CancellationToken ct = default);
    Task<List<SelectListItemMatch>> SelectListItemMatchesAsync(string commandText, CancellationToken ct = default);
    Task<IEnumerable<T>> SelectListItemAsync<T>(string commandText, Func<SqlDataReader, T> mapFunc, CancellationToken ct = default);
    Task<IEnumerable<T>> SelectListItemAsync<T>(string commandText, Dictionary<string, string>? sqlParams, Func<SqlDataReader, T> mapFunc, CancellationToken ct = default);

}
