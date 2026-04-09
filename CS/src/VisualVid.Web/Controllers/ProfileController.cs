using Microsoft.AspNetCore.Mvc;
using VisualVid.Web.Models.ViewModels;
using VisualVid.Web.Services;

namespace VisualVid.Web.Controllers;

public class ProfileController : Controller
{
    private readonly MemberService _memberService;
    private readonly VideoService _videoService;

    public ProfileController(MemberService memberService, VideoService videoService)
    {
        _memberService = memberService;
        _videoService = videoService;
    }

    public async Task<IActionResult> Index(Guid? userId, string? UserId)
    {
        // Support both ?userId= and legacy ?UserId= parameter
        var id = userId ?? (Guid.TryParse(UserId, out var parsed) ? parsed : (Guid?)null);
        if (id == null)
            return RedirectToAction("Index", "Home");

        var member = await _memberService.GetByUserIdAsync(id.Value);
        if (member == null)
            return RedirectToAction("Index", "Home");

        var videos = await _videoService.GetByUserAsync(id.Value, activeOnly: true);

        var model = new PublicProfileViewModel
        {
            UserId = member.UserId,
            UserName = member.User?.UserName,
            PhotoUrl = $"/videos/members/{member.UserId}.jpg",
            Location = member.Country?.Name,
            AboutMe = null, // No about-me field in legacy schema
            MemberSince = member.DateCreated?.ToString("MMMM d, yyyy"),
            Videos = videos
        };

        return View(model);
    }
}
