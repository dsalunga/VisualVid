using Microsoft.AspNetCore.Mvc;
using VisualVid.Web.Models.ViewModels;
using VisualVid.Web.Services;

namespace VisualVid.Web.Controllers;

public class SearchController : Controller
{
    private readonly VideoService _videoService;

    public SearchController(VideoService videoService)
    {
        _videoService = videoService;
    }

    public async Task<IActionResult> Index(string? q, string? Search, string? sort)
    {
        // Support both new ?q= and legacy ?Search= parameter
        var query = q ?? Search;

        var videos = string.IsNullOrWhiteSpace(query)
            ? []
            : await _videoService.SearchAsync(query, sort);

        var model = new SearchResultsViewModel
        {
            Query = query,
            Sort = sort,
            Videos = videos
        };

        return View(model);
    }
}
