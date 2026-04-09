using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VisualVid.Core.Helpers;
using VisualVid.Web.Models;
using VisualVid.Web.Models.ViewModels;
using VisualVid.Web.Services;

namespace VisualVid.Web.Controllers;

public class VideoController : Controller
{
    private readonly VideoService _videoService;
    private readonly MemberService _memberService;
    private readonly UserManager<ApplicationUser> _userManager;

    public VideoController(VideoService videoService, MemberService memberService,
        UserManager<ApplicationUser> userManager)
    {
        _videoService = videoService;
        _memberService = memberService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Watch(Guid videoId)
    {
        var video = await _videoService.GetByIdAsync(videoId, incrementViews: true);
        if (video == null)
            return RedirectToAction("Index", "Home");

        // Increment member watched count if authenticated
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
                await _memberService.IncrementWatchedAsync(user.Id);
        }

        var comments = await _videoService.GetCommentsAsync(videoId);

        var model = new WatchViewModel
        {
            Video = new WatchVideoDetail
            {
                VideoId = video.VideoId,
                Title = video.Title,
                Description = video.Description,
                VideoUrl = $"/videos/members/{video.UserId}/{video.VideoId}",
                DateAdded = video.DateAdded?.ToString("MMMM d, yyyy"),
                UserName = video.User?.UserName,
                UserId = video.UserId ?? Guid.Empty,
                Views = video.Views,
                TagsHtml = VideoHelper.FormatTags(video.Tags),
                CategoryName = video.Category?.Name
            },
            Comments = comments,
            RelatedVideos = []
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(Guid videoId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return RedirectToAction(nameof(Watch), new { videoId });

        var user = await _userManager.GetUserAsync(User);
        if (user != null)
            await _videoService.AddCommentAsync(videoId, user.Id, content.Trim());

        return RedirectToAction(nameof(Watch), new { videoId });
    }
}
