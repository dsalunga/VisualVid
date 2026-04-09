using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VisualVid.Web.Models;
using VisualVid.Web.Services;

namespace VisualVid.Web.Controllers;

public class HomeController : Controller
{
    private readonly VideoService _videoService;

    public HomeController(VideoService videoService)
    {
        _videoService = videoService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.FeaturedVideos = await _videoService.GetFeaturedVideosAsync();
        ViewBag.MostWatched = await _videoService.GetMostWatchedAsync();
        return View();
    }

    public IActionResult About() => View();
    public IActionResult Contact() => View();
    public IActionResult Privacy() => View();
    public IActionResult Terms() => View();
    public IActionResult Sitemap() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
