using Microsoft.AspNetCore.Mvc;
using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Models.ViewModels;
using PremierLeague_Backend.Helper.SqlCommands;
using Microsoft.AspNetCore.Authorization;

namespace PremierLeague_Backend.Controllers
{
    [Authorize]
    [Route("en/player-stats")]
    public class PlayerStatController : Controller
    {
        private readonly ILogger<PlayerStatController> _logger;
        private readonly IPlayerStatRepository repository;
        private readonly ISelectListItems selectList;
        private readonly PlayerStatViewModel viewModel;

        public PlayerStatController(ILogger<PlayerStatController> logger, IPlayerStatRepository repository, ISelectListItems selectList)
        {
            this._logger = logger;
            this.repository = repository;
            this.selectList = selectList;
            this.viewModel = new PlayerStatViewModel();
        }

        // GET: PlayerStatController
        [HttpGet]
        public async Task<ActionResult> Index(List<int>? club, int season, int week = 0, int? page = 1)
        {
            try
            {
                viewModel.PlayerStatMatchListDtos = await repository.GetPlayerStatMatchListAsync(season, week, page);
                viewModel.StatCategoriesSelectListItem = await selectList.SelectListItemsAsync(SelectListItemCommands.CommandSelectListItemStatCategoriesForMatchKickoffCommandText);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading player stats with filters");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        // POST: PlayerStatController/Create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PlayerStatViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating player stat");
                    return View("Index", viewModel);
                }

                var success = await repository.AddPlayerStatAsync(model.PlayerStatDto);
                if (!success)
                {
                    _logger.LogError("Failed to add player stat");
                    ModelState.AddModelError(string.Empty, "Failed to add player stat.");
                    return View("Index", viewModel);
                }

                TempData["SuccessMessage"] = "Player stat added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating player stat");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", viewModel);
            }
        }

        // POST: PlayerStatController/Update

        // POST: PlayerStatController/Delete

        // GET: GetStatsController
        [HttpGet("get-stats/{statCategoryId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetStatItemsByStatCategoryBy([FromRoute] int statCategoryId)
        {
            try
            {
                var param = new Dictionary<string, string>()
                {
                    // {"@StatScopeId", "1"},
                    {"@CategoryId", statCategoryId.ToString()},
                };

                var StatsSelectListItem = await selectList.SelectListItemsAsync(SelectListItemCommands.CommandSelectListItemStatsCommandText, param);
                return Json(new
                {
                    StatusCode = 200,
                    Message = "Commit Transaction Success.",
                    Data = new
                    {
                        StatsSelectListItem
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error find stats on {statCategoryId}", statCategoryId);
                ModelState.AddModelError(string.Empty, ex.Message);
                return Json(new
                {
                    StatusCode = 400,
                    Message = ex.Message,
                });
            }
        }

        // GET: GetPlayersController
        [HttpGet("get-players/{matchId:int}/{clubId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetPlayerItemsForStat([FromRoute] int matchId, [FromRoute] int clubId, int statId, bool isHomeClub)
        {
            try
            {
                var stats = await repository.GetStatByIdForPlayerStatAsync(statId);
                var (isPlayerStat, isClubStat, symbol) = stats.Where(stat => stat.StatId == statId)
                .Select(stat => (stat.IsPlayerStat, stat.IsClubStat, stat.Symbol))
                .FirstOrDefault();

                if (isPlayerStat)
                {
                    viewModel.PlayerStatGetPlayersDtos = isHomeClub ?
                    await repository.GetHomePlayersForPlayerStatByClubIdAndMatchIdAsync(matchId, clubId) :
                    await repository.GetAwayPlayersForPlayerStatByClubIdAndMatchIdAsync(matchId, clubId);
                }

                return Json(new
                {
                    StatusCode = 200,
                    Message = "Commit Transaction Success.",
                    Data = new
                    {
                        isPlayerStat,
                        isClubStat,
                        symbol,
                        PlayerItems = viewModel.PlayerStatGetPlayersDtos
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error find players on {clubId}", clubId);
                ModelState.AddModelError(string.Empty, ex.Message);
                return Json(new
                {
                    StatusCode = 400,
                    Message = ex.Message,
                });
            }
        }
    
    }
}
