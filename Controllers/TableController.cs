using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Models.ViewModels;

namespace PremierLeague_Backend.Controllers
{
    [Authorize]
    [Route("en/tables")]
    public class TableController : Controller
    {
        private readonly ILogger<TableController> _logger;
        private readonly ITableRepository repository;
        private TableViewModel viewModel;

        public TableController(ILogger<TableController> logger, ITableRepository repository)
        {
            this._logger = logger;
            this.repository = repository;
            this.viewModel = new TableViewModel();
        }

        // GET: TableController
        public async Task<ActionResult> Index()
        {
            try
            {
                viewModel.Tables = await repository.GetTableAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching match info list: {Message}", ex.Message);
                ModelState.AddModelError(string.Empty, "Unable to load match info list. Please try again later or contact admin.");
            }
            return View(viewModel);
        }
    }
}
