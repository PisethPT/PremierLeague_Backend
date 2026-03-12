using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

        // POST: LineupController/Create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync()
        {
            try
            {
                return View(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lineup");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
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
    }
}
