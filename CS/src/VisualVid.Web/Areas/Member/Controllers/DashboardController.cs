using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VisualVid.Web.Models;
using VisualVid.Web.Models.ViewModels;
using VisualVid.Web.Services;

namespace VisualVid.Web.Areas.Member.Controllers;

[Area("Member")]
[Authorize]
public class DashboardController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MemberService _memberService;
    private readonly VideoService _videoService;

    public DashboardController(
        UserManager<ApplicationUser> userManager,
        MemberService memberService,
        VideoService videoService)
    {
        _userManager = userManager;
        _memberService = memberService;
        _videoService = videoService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account", new { area = "" });

        var member = await _memberService.GetByUserIdAsync(user.Id);
        var videos = await _videoService.GetByUserAsync(user.Id);

        var model = new MemberDashboardViewModel
        {
            UserName = user.UserName,
            UserId = user.Id,
            ProfileImageUrl = $"/videos/members/{user.Id}.jpg",
            Age = member != null ? MemberService.CalculateAge(member.BirthDate) : 0,
            Gender = member?.Gender == true ? "Male" : "Female",
            Email = user.Email,
            Country = member?.Country?.Name,
            Watched = member?.Watched ?? 0,
            TotalVideos = videos.Count,
            TotalViews = videos.Sum(v => v.Views),
            Videos = videos
        };

        return View(model);
    }
}
