using Microsoft.AspNetCore.Mvc;
using PremierLeague_Backend.Data.Repositories.Interfaces;
using PremierLeague_Backend.Models.ViewModels;
using PremierLeague_Backend.Helper.SqlCommands;
using PremierLeague_Backend.Models.DTOs;
using PremierLeague_Backend.Helper;

namespace PremierLeague_Backend.Controllers
{
    [Route("en/news")]
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsRepository repository;
        private readonly ISelectListItems selectListItems;
        private readonly NewsViewModel viewModel;

        public NewsController(ILogger<NewsController> logger, INewsRepository repository, ISelectListItems selectListItems)
        {
            this._logger = logger;
            this.repository = repository;
            this.selectListItems = selectListItems;
            this.viewModel = new NewsViewModel();
        }

        // GET: NewsController
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                viewModel.NewsDetailDtos = await repository.GetAllNewsDetailAsync();
                viewModel.SelectListItemNewsTag = await selectListItems.SelectListItemHasSubtitleAsync(SelectListItemCommands.CommandSelectListItemNewsTag);
                ViewBag.TotalCount = viewModel.NewsDetailDtos.Count();
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = (int)Math.Ceiling((double)ViewBag.TotalCount / 10);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading news");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        // POST: NewsController/Create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([FromForm] NewsDto newsDto)
        {
            try
            {
                newsDto.AuthorId = "12c64674-5ea2-484d-90b1-7419243b1758"; // TODO: Get user id from session
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

                var validation = FileValidator.Validate(newsDto.ImageFile);
                if (!validation.IsValid)
                {
                    ModelState.AddModelError(nameof(newsDto.ImageFile), validation.ErrorMessage ?? "Invalid file");
                    return RedirectToAction(nameof(Index));
                }

                var success = await repository.AddNewsAsync(newsDto);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Unable to create news. Please try again or contact admin.");
                    _logger.LogWarning("AddNewsAsync returned {success} for news {Title}, Author: {Author}", success, newsDto.Title, newsDto.AuthorId);
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "News create successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating news");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(nameof(Index), viewModel);
            }
        }

        // POST: NewsController/Update
        [HttpPost("update/{newsId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateAsync([FromRoute] int newsId, [FromForm] NewsDto newsDto)
        {
            try
            {
                newsDto.AuthorId = "12c64674-5ea2-484d-90b1-7419243b1758"; // TODO: Get user id from session
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

                if (newsDto.ImageFile is not null || newsDto.ImageFile?.Length > 0)
                {
                    var validation = FileValidator.Validate(newsDto.ImageFile);
                    if (!validation.IsValid)
                    {
                        ModelState.AddModelError(nameof(newsDto.ImageFile), validation.ErrorMessage ?? "Invalid file");
                        return RedirectToAction(nameof(Index));
                    }
                }

                var success = await repository.UpdateNewsAsync(newsDto);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Unable to update news. Please try again or contact admin.");
                    _logger.LogWarning("UpdateNewsAsync returned {success} for news {NewsId} {Title}, Author: {Author}", success, newsId, newsDto.Title, newsDto.AuthorId);
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "News updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating news");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(nameof(Index), viewModel);
            }
        }

        // POST: NewsController/Delete
        [HttpPost("delete/{newsId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync([FromRoute] int newsId, [FromForm] string? imageUrl)
        {
            try
            {
                var success = await repository.DeleteNewsAsync(newsId, imageUrl);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Unable to delete news. Please try again or contact admin.");
                    _logger.LogWarning("DeleteNewsAsync returned {success} for news {NewsId}", success, newsId);
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "News deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting news");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(nameof(Index), viewModel);
            }
        }

        // GET: NewsController/GetNewsById
        [HttpGet("get-news/{newsId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetNewsByIdAsync([FromRoute] int newsId)
        {
            try
            {
                var newsDto = await repository.GetNewsByIdAsync(newsId);
                return Json(new
                {
                    StatusCode = 200,
                    Message = "Commit Transaction Success.",
                    Data = new
                    {
                        NewsItem = newsDto
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting news by id");
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
