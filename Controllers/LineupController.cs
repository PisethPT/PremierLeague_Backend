using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Backend.Models.DTOs;
using System.Text.Json;

namespace PremierLeague_Backend.Controllers
{
    [Route("en/lineups")]
    public class LineupController : Controller
    {
        private readonly ILogger<LineupController> _logger;
        private readonly ILineupRepository repository;
        private readonly ISelectListItems selectList;
        private readonly LineupViewModel viewModel;

        public LineupController(ILogger<LineupController> logger, ILineupRepository repository, ISelectListItems selectList)
        {
            this._logger = logger;
            this.repository = repository;
            this.selectList = selectList;
            this.viewModel = new();
        }

        // GET: LineupController
        [HttpGet]
        public async Task<ActionResult> IndexAsync(int? page = 1)
        {
            try
            {
                viewModel.LineupDetailDto = await repository.GetAllLineupsAsync(page);
                viewModel.SelectListItemMatchForLineups = await selectList.SelectListItemMatchForLineupAsync();
                ViewBag.TotalCount = viewModel.LineupDetailDto.Count();
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)ViewBag.TotalCount / 20);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading lineup");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        // POST: LineupController/CreateAsync
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([FromBody] LineupDto lineupDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Invalid data provided.";
                    _logger.LogWarning("Invalid data provided for lineup creation.");
                    return BadRequest(new { redirectUrl = Url.Action("Index") });
                }

                var isExist = await repository.IsExistLineupAsync(lineupDto.MatchId);
                if (isExist)
                {
                    TempData["ErrorMessage"] = "Lineup already exists.";
                    _logger.LogWarning("Lineup already exists for match {MatchId}", lineupDto.MatchId);
                    return Conflict(new { redirectUrl = Url.Action("Index") });
                }

                var success = await repository.AddLineupAsync(lineupDto);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Error saving to database.";
                    _logger.LogWarning("Error saving lineup to database.");
                    return StatusCode(500, new { redirectUrl = Url.Action("Index") });
                }

                TempData["SuccessMessage"] = $"Lineup created successfully for Match {lineupDto.MatchId}";
                _logger.LogInformation("Lineup created successfully for Match {MatchId}", lineupDto.MatchId);
                return Ok(new { redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Error creating lineup for match {MatchId}", lineupDto.MatchId);
                return StatusCode(500, new { redirectUrl = Url.Action("Index") });
            }
        }

        // POST: LineupController/UpdateAsync
        [HttpPost("update/{matchId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync([FromRoute] int matchId, [FromBody] LineupDto lineupDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Invalid data provided.";
                    _logger.LogWarning("Invalid data provided for lineup update.");
                    return BadRequest(new { redirectUrl = Url.Action("Index") });
                }

                var isMatchCurringKickOff = await repository.IsMatchCurringKickOffAsync(matchId);
                if (isMatchCurringKickOff)
                {
                    TempData["ErrorMessage"] = "Match is curring kick off. You can't update lineup.";
                    _logger.LogWarning("Match is curring kick off for match {MatchId} and can't update lineup.", matchId);
                    return BadRequest(new { redirectUrl = Url.Action("Index") });
                }

                var isMatchEnded = await repository.IsMatchEndedAsync(matchId);
                if (isMatchEnded)
                {
                    TempData["ErrorMessage"] = "Match is ended. You can't update lineup.";
                    _logger.LogWarning("Match is ended for match {MatchId} and can't update lineup.", matchId);
                    return BadRequest(new { redirectUrl = Url.Action("Index") });
                }

                var isExist = await repository.IsExistLineupAsync(matchId);
                if (!isExist)
                {
                    TempData["ErrorMessage"] = "Lineup not found.";
                    _logger.LogWarning("Lineup not found for match {MatchId}", matchId);
                    return NotFound(new { redirectUrl = Url.Action("Index") });
                }

                var success = await repository.UpdateLineupAsync(matchId, lineupDto);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Error saving to database.";
                    _logger.LogWarning("Error saving lineup to database.");
                    return StatusCode(500, new { redirectUrl = Url.Action("Index") });
                }

                TempData["SuccessMessage"] = $"Lineup updated successfully for Match {matchId}";
                _logger.LogInformation("Lineup updated successfully for Match {MatchId}", matchId);
                return Ok(new { redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Error updating lineup for match {MatchId}", matchId);
                return StatusCode(500, new { redirectUrl = Url.Action("Index") });
            }
        }

        // POST: LineupController/DeleteAsync
        [HttpPost("delete/{matchId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync([FromRoute] int matchId)
        {
            try
            {
                var isMatchCurringKickOff = await repository.IsMatchCurringKickOffAsync(matchId);
                if (isMatchCurringKickOff)
                {
                    TempData["ErrorMessage"] = "Match is curring kick off. You can't delete lineup.";
                    _logger.LogWarning("Match is curring kick off for match {MatchId} and can't delete lineup.", matchId);
                    return BadRequest(new { redirectUrl = Url.Action("Index") });
                }

                var isMatchEnded = await repository.IsMatchEndedAsync(matchId);
                if (isMatchEnded)
                {
                    TempData["ErrorMessage"] = "Match is ended. You can't delete lineup.";
                    _logger.LogWarning("Match is ended for match {MatchId} and can't delete lineup.", matchId);
                    return BadRequest(new { redirectUrl = Url.Action("Index") });
                }

                var isExist = await repository.IsExistLineupAsync(matchId);
                if (!isExist)
                {
                    TempData["ErrorMessage"] = "Lineup not found.";
                    _logger.LogWarning("Lineup not found for match {MatchId}", matchId);
                    return NotFound(new { redirectUrl = Url.Action("Index") });
                }

                var success = await repository.DeleteLineupAsync(matchId);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Error saving to database.";
                    _logger.LogWarning("Error saving lineup to database.");
                    return StatusCode(500, new { redirectUrl = Url.Action("Index") });
                }

                TempData["SuccessMessage"] = $"Lineup deleted successfully for Match {matchId}";
                _logger.LogInformation("Lineup deleted successfully for Match {MatchId}", matchId);
                return Ok(new { redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Error deleting lineup for match {MatchId}", matchId);
                return StatusCode(500, new { redirectUrl = Url.Action("Index") });
            }
        }

        // GET: LineupController/GetLineupByMatchId
        [HttpGet("get-lineup-by-match-id/{matchId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetLineupByMatchIdAsync([FromRoute] int matchId)
        {
            try
            {
                var lineup = await repository.GetLineupByMatchIdAsync(matchId);
                return Json(
                    new
                    {
                        StatusCode = 200,
                        Data = new
                        {
                            Lineup = lineup,
                        },
                        Message = "Commit Transaction Success."
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lineup for match {MatchId}", matchId);
                return Json(
                    new
                    {
                        StatusCode = 400,
                        Message = ex.Message,
                    }
                );
            }
        }

        // GET: LineupController/GetFormations
        [HttpGet("get-formations")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetFormationsAsync()
        {
            try
            {
                viewModel.SelectListItemFormations = await selectList.SelectListItemFormationAsync();

                return Json(
                    new
                    {
                        StatusCode = 200,
                        Data = new
                        {
                            ListItem = viewModel.SelectListItemFormations,
                        },
                        Message = "Commit Transaction Success."
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading formations");
                return Json(
                    new
                    {
                        StatusCode = 400,
                        Message = ex.Message,
                    }
                );
            }
        }

        // GET: LineupController/GetLineupFormationLayoutByFormationId
        [HttpGet("get-lineup-formation-layout-by-formation-id/{formationId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetLineupFormationLayoutByFormationIdAsync([FromRoute] int formationId)
        {
            try
            {
                var ListItem = await repository.GetLineupFormationLayoutByFormationIdAsync(formationId);
                return Json(
                    new
                    {
                        StatusCode = 200,
                        Data = new
                        {
                            ListItem,
                        },
                        Message = "Commit Transaction Success."
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading formations");
                return Json(
                    new
                    {
                        StatusCode = 400,
                        Message = ex.Message,
                    }
                );
            }
        }

        // POST: LineupController/GetLineupClubInfoByMatch
        [HttpGet("get-lineup-club-info-by-match/{matchId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetLineupClubInfoByMatchAsync([FromRoute] int matchId)
        {
            try
            {
                var LineupClubInfo = await repository.GetLineupClubInfoByMatchIdAsync(matchId);
                if (LineupClubInfo is null)
                {
                    return Json(new
                    {
                        StatusCode = 404,
                        Message = $"Lineup club information not found. by matchId {matchId}",
                    });
                }

                return Json(
                    new
                    {
                        StatusCode = 200,
                        Data = new
                        {
                            LineupClubInfo
                        },
                        Message = "Commit Transaction Success."
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading players by match Id");
                return Json(
                    new
                    {
                        StatusCode = 400,
                        Message = ex.Message,
                    }
                );
            }
        }

        // POST: LineupController/GetLineupClubInfoByMatch
        [HttpGet("get-lineup-formation-detail-by-match/{matchId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetLineupFormationDetailByMatchAsync([FromRoute] int matchId)
        {
            try
            {
                var LineupFormation = await repository.GetLineupFormationByMatchIdAsync(matchId);
                var SubstitutionFormation = await repository.GetSubstitutionFormationByMatchIdAsync(matchId);
                if (LineupFormation is null || SubstitutionFormation is null)
                {
                    return Json(new
                    {
                        StatusCode = 404,
                        Message = $"Lineup information detail not found. by matchId {matchId}",
                    });
                }

                return Json(
                    new
                    {
                        StatusCode = 200,
                        Data = new
                        {
                            LineupFormation,
                            SubstitutionFormation
                        },
                        Message = "Commit Transaction Success."
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading players by match Id");
                return Json(
                    new
                    {
                        StatusCode = 400,
                        Message = ex.Message,
                    }
                );
            }
        }

        // POST: LineupController/GetPlayersByMatch
        [HttpGet("get-players-by-match/{matchId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetPlayersByMatchAsync([FromRoute] int matchId)
        {
            try
            {
                var (homeClubId, awayClubId) = await repository.GetHomeClubAndAwayClubByMatchIdAsync(matchId);
                if (homeClubId == 0 || awayClubId == 0)
                {
                    throw new Exception("Match not found.");
                }

                viewModel.SelectListItemHomeClubPlayer = await selectList.SelectListItemPlayerLineupByClubIdAsync(matchId, homeClubId);
                viewModel.SelectListItemAwayClubPlayer = await selectList.SelectListItemPlayerLineupByClubIdAsync(matchId, awayClubId);

                return Json(
                    new
                    {
                        StatusCode = 200,
                        Data = new
                        {
                            HomePlayers = viewModel.SelectListItemHomeClubPlayer,
                            AwayPlayers = viewModel.SelectListItemAwayClubPlayer,
                        },
                        Message = "Commit Transaction Success."
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading players by match Id {matchId}");
                return Json(
                    new
                    {
                        StatusCode = 400,
                        Message = ex.Message,
                    }
                );
            }
        }
    }
}
