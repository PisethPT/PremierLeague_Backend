using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PremierLeague_Backend.Controllers;

[Authorize]
[Route("en/match-info")]
public class MatchInfoController : Controller
{
    private readonly ILogger<MatchInfoController> _logger;

    public MatchInfoController(ILogger<MatchInfoController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync()
    {
        return View();
    }

    [HttpPost("update/{matchId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAsync([FromRoute] int matchId)
    {
        return View();
    }

    [HttpPost("delete/{matchId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAsync([FromRoute] int matchId)
    {
        return View();
    }

    [HttpGet("get-match-info/{matchId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetMatchInfoAsync([FromRoute] int matchId)
    {
        return View();
    }
}