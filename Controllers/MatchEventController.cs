using Microsoft.AspNetCore.Mvc;
using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Models.ViewModels;
using PremierLeague_Backend.Helper.SqlCommands;
using PremierLeague_Backend.Models.DTOs;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace PremierLeague_Backend.Controllers
{
    [Authorize]
    [Route("en/match-events")]
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
        public async Task<ActionResult> IndexAsync(List<int>? club, int season, int week = 0, int? page = 1)
        {
            try
            {
                viewModel.MatchEventDuringMatchWeekDtos = await repository.GetMatchEventDuringMatchWeekAsync(season, week, page);

                viewModel.MatchEventTypesSelectListItem = await selectList.SelectListItemsAsync(SelectListItemCommands.CommandSelectListItemMatchEventType);
                viewModel.MatchEventOutcomesSelectListItem = await selectList.SelectListItemsAsync(SelectListItemCommands.CommandSelectListItemMatchEventOutcome);

                viewModel.MatchEventTypesDtos = await repository.GetMatchEventTypesAsync();
                viewModel.MatchEventDetailDtos = await repository.GetMatchEventDetailByMatchIdAsync(1);

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
        public async Task<ActionResult> CreateAsync([FromForm] MatchEventDto matchEventDto)
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

                var isExistingMatchEvent = await repository.FindExistingMatchEventPerClubIdAndMatchIdAsync(matchEventDto.ClubId, matchEventDto.MatchId, matchEventDto.PlayerId, matchEventDto.Minute!);
                if (!isExistingMatchEvent)
                {
                    ModelState.AddModelError(string.Empty, "Unable to create match event. This match event is existing.");
                    _logger.LogWarning("FindExistingMatchEventPerClubIdAndMatchIdAsync returned {success} for match event {MatchId}", isExistingMatchEvent, matchEventDto.MatchId);
                    return RedirectToAction(nameof(Index));
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
                return View(nameof(Index), viewModel);
            }
        }

        // POST: MatchEventController/Update
        [HttpPost("update/{matchEventId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateAsync([FromRoute] int matchEventId, [FromForm] MatchEventDto matchEventDto)
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

                var isExistsMatchEvent = await repository.FindExistsMatchEventByMatchEventIdAsync(matchEventDto);
                if (!isExistsMatchEvent)
                {
                    ModelState.AddModelError(string.Empty, "Unable to update match event already exists for the same match, club, player and minute.");
                    _logger.LogWarning("FindExistsMatchEventByMatchEventIdAsync returned {success} for match event {MatchId}", isExistsMatchEvent, matchEventDto.MatchId);
                    return RedirectToAction(nameof(Index));
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

        // POST: MatchEventController/Delete
        [HttpPost("delete/{matchEventId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync([FromRoute] int matchEventId)
        {
            try
            {
                var matchEvent = await repository.FindMatchEventByIdAsync(matchEventId);
                if (matchEvent is null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to find match event. Please try again or contact admin.");
                    _logger.LogWarning("FindMatchEventByIdAsync returned null for match event {MatchId}", matchEventId);
                    return RedirectToAction(nameof(Index));
                }

                var success = await repository.DeleteMatchEventAsync(matchEventId);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Unable to delete match event. Please try again or contact admin.");
                    _logger.LogWarning("DeleteMatchEventAsync returned {success} for match event {MatchId}", success, matchEventId);
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Match event delete successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading match events with filters");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        // GET: MatchEventController/GetMatchEventByMatchId
        [HttpGet("get-match-event/{matchEventId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetMatchEventByMatchIdAsync([FromRoute] int matchEventId)
        {
            try
            {
                var matchEvent = await repository.FindMatchEventByIdAsync(matchEventId);
                return Json(new
                {
                    StatusCode = 200,
                    Message = "Commit Transaction Success.",
                    Data = new
                    {
                        MatchEvent = matchEvent
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading match events with filters");
                ModelState.AddModelError(string.Empty, ex.Message);
                return Json(new
                {
                    StatusCode = 400,
                    Message = ex.Message,
                });
            }
        }

        // GET: MatchEventController/GetPlayerItems
        [HttpGet("get-players/{matchId:int}/{clubId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetPlayerItemsAsync([FromRoute] int matchId, [FromRoute] int clubId, bool isHomeClub)
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

        // GET: MatchEventController/GetMatchEventByMatchId
        [HttpGet("get-match-event-types/{matchId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetMatchEventTypesByMatchIdAsync([FromRoute] int matchId)
        {
            try
            {
                var matchEvent = await repository.GetMatchEventTypesAsync();
                var matchEventTypeDetail = await repository.GetMatchEventDetailByMatchIdAsync(matchId);
                return Json(new
                {
                    StatusCode = 200,
                    Message = "Commit Transaction Success.",
                    Data = new
                    {
                        MatchEvent = matchEvent,
                        MatchEventTypeDetail = matchEventTypeDetail,
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading match events with filters");
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
