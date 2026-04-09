using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VisualVid.Core.Helpers;
using VisualVid.Web.Models;
using VisualVid.Web.Models.ViewModels;
using VisualVid.Web.Services;
using System.Diagnostics;

namespace VisualVid.Web.Areas.Member.Controllers;

[Area("Member")]
[Authorize]
public class VideoController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly VideoService _videoService;
    private readonly MemberService _memberService;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public VideoController(
        UserManager<ApplicationUser> userManager,
        VideoService videoService,
        MemberService memberService,
        IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _videoService = videoService;
        _memberService = memberService;
        _environment = environment;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Upload()
    {
        ViewBag.Categories = (await _memberService.GetCategoriesAsync())
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(c.Name, c.CategoryId.ToString()));
        return View(new VideoUploadViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(536_870_912)] // 512 MB
    public async Task<IActionResult> Upload(VideoUploadViewModel model)
    {
        ViewBag.Categories = (await _memberService.GetCategoriesAsync())
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(c.Name, c.CategoryId.ToString()));

        if (!ModelState.IsValid || model.VideoFile == null)
            return View(model);

        var maxSizeMb = _configuration.GetValue<int>("AppSettings:MaxVideoSizeMB", 100);
        var fileSizeMb = model.VideoFile.Length / (1024 * 1024);
        if (fileSizeMb > maxSizeMb)
        {
            ModelState.AddModelError(string.Empty,
                $"Maximum video size is {maxSizeMb}MB. Your file is {fileSizeMb}MB.");
            return View(model);
        }

        // Validate file extension
        var allowedVideoExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".webm", ".m4v" };
        var extension = Path.GetExtension(model.VideoFile.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !allowedVideoExtensions.Contains(extension))
        {
            ModelState.AddModelError(string.Empty,
                "Invalid video file type. Allowed types: " + string.Join(", ", allowedVideoExtensions));
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        var videoId = await _videoService.CreateAsync(
            model.Title, model.Description, model.Tags,
            model.CategoryId, extension, user.Id);

        // Save pending file
        var videoStoragePath = _configuration["AppSettings:VideoStoragePath"] ?? "wwwroot/videos";
        var pendingDir = Path.Combine(_environment.ContentRootPath, videoStoragePath, "pending");
        if (!Directory.Exists(pendingDir))
            Directory.CreateDirectory(pendingDir);

        var pendingPath = Path.Combine(pendingDir, $"{videoId}{extension}");
        await using (var stream = new FileStream(pendingPath, FileMode.Create))
        {
            await model.VideoFile.CopyToAsync(stream);
        }

        // Ensure member directory exists
        var memberDir = Path.Combine(_environment.ContentRootPath, videoStoragePath, "members", user.Id.ToString());
        if (!Directory.Exists(memberDir))
            Directory.CreateDirectory(memberDir);

        return RedirectToAction("UploadComplete", new { videoId });
    }

    [HttpGet]
    public IActionResult UploadComplete(Guid videoId)
    {
        ViewBag.VideoId = videoId;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid videoId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        var video = await _videoService.GetByIdAsync(videoId);
        if (video == null || video.UserId != user.Id)
            return RedirectToAction("Index", "Dashboard");

        ViewBag.Categories = (await _memberService.GetCategoriesAsync())
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(c.Name, c.CategoryId.ToString()));

        var model = new VideoEditViewModel
        {
            VideoId = video.VideoId,
            Title = video.Title ?? string.Empty,
            Description = video.Description,
            Tags = video.Tags,
            CategoryId = video.CategoryId ?? 0,
            ThumbnailUrl = $"/videos/members/{user.Id}/{video.VideoId}.jpg",
            WatchUrl = $"/Video/Watch?videoId={video.VideoId}"
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(VideoEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = (await _memberService.GetCategoriesAsync())
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(c.Name, c.CategoryId.ToString()));
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        var video = await _videoService.GetByIdAsync(model.VideoId);
        if (video == null || video.UserId != user.Id)
            return Forbid();

        await _videoService.UpdateAsync(model.VideoId, model.Title, model.Description, model.Tags, model.CategoryId);
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid videoId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        var video = await _videoService.GetByIdAsync(videoId);
        if (video == null || video.UserId != user.Id)
            return Forbid();

        await _videoService.DeleteAsync(videoId);

        // Delete video files
        var videoStoragePath = _configuration["AppSettings:VideoStoragePath"] ?? "wwwroot/videos";
        var basePath = Path.Combine(_environment.ContentRootPath, videoStoragePath, "members", user.Id.ToString(), videoId.ToString());
        try
        {
            if (System.IO.File.Exists(basePath + ".flv")) System.IO.File.Delete(basePath + ".flv");
            if (System.IO.File.Exists(basePath + ".mp4")) System.IO.File.Delete(basePath + ".mp4");
            if (System.IO.File.Exists(basePath + ".jpg")) System.IO.File.Delete(basePath + ".jpg");
        }
        catch { }

        return RedirectToAction("Index", "Dashboard");
    }
}
