using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Models.ViewModels;

public class TableViewModel
{
    public IEnumerable<TableDto> Tables { get; set; }
    public TableViewModel()
    {
        this.Tables = new List<TableDto>();
    }
}
