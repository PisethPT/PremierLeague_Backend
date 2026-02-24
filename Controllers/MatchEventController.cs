using Microsoft.AspNetCore.Mvc;
using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Models.ViewModels;
using PremierLeague_Backend.Helper.SqlCommands;
using PremierLeague_Backend.Models.DTOs;

namespace PremierLeague_Backend.Controllers
{
    [Route("match-events")]
    public class MatchEventController : Controller
    {
        private readonly ILogger<MatchEventController> _logger;
        private readonly IMatchEventRepository repository;
        public ISelectListItems selectList { get; }
        private readonly MatchEventViewModel viewModel;

        public MatchEventController(ILogger<MatchEventController> logger, IMatchEventRepository repository, ISelectListItems selectList)
        {
            this._logger = logger;
            this.repository = repository;
            this.selectList = selectList;
            this.viewModel = new MatchEventViewModel();
        }

        // GET: MatchEventController
        [HttpGet]
        public async Task<ActionResult> Index(List<int>? club, int season, int week = 0, int? page = 1)
        {
            try
            {
                viewModel.MatchEventDuringMatchWeekDtos = await repository.GetMatchEventDuringMatchWeekAsync(season, week, page);
                viewModel.MatchEventTypesDtos = await repository.GetMatchEventTypesAsync(1);

                viewModel.MatchEventTypesSelectListItem = await selectList.SelectListItemsAsync(SelectListItemCommands.CommandSelectListItemMatchEventType);
                viewModel.MatchEventOutcomesSelectListItem = await selectList.SelectListItemsAsync(SelectListItemCommands.CommandSelectListItemMatchEventOutcome);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading match events with filters");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        // POST: MatchEventController/Create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] MatchEventDto matchEventDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(ms => ms.Value!.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    throw new Exception($"Form validation failed: {string.Join(" | ", errors.Select(e => $"{e.Key}: {string.Join(", ", e.Value)}"))}");
                }

                var success = await repository.AddMatchEventAsync(matchEventDto);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Unable to create match event. Please try again or contact admin.");
                    _logger.LogWarning("AddMatchEventAsync returned {success} for match event {MatchId}", success, matchEventDto.MatchId);
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Match event create successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading match events with filters");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        // POST: MatchEventController/Update
        [HttpPost("update/{matchEventId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update([FromRoute] int matchEventId, [FromForm] MatchEventDto matchEventDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(ms => ms.Value!.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    throw new Exception($"Form validation failed: {string.Join(" | ", errors.Select(e => $"{e.Key}: {string.Join(", ", e.Value)}"))}");
                }

                var success = await repository.UpdateMatchEventAsync(matchEventId, matchEventDto);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Unable to update match event. Please try again or contact admin.");
                    _logger.LogWarning("UpdateMatchEventAsync returned {success} for match event {MatchId}", success, matchEventDto.MatchId);
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Match event update successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading match events with filters");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }
        
        // GET: MatchEventController
        [HttpGet("get-players/{matchId:int}/{clubId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetPlayerItems([FromRoute] int matchId, [FromRoute] int clubId, bool isHomeClub)
        {
            try
            {
                viewModel.PlayerStatGetPlayersDtos = isHomeClub ?
                    await repository.GetHomePlayersForPlayerStatByClubIdAndMatchIdAsync(matchId, clubId) :
                    await repository.GetAwayPlayersForPlayerStatByClubIdAndMatchIdAsync(matchId, clubId);

                return Json(new
                {
                    StatusCode = 200,
                    Message = "Commit Transaction Success.",
                    Data = new
                    {
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
