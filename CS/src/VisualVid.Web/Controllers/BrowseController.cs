using Microsoft.AspNetCore.Mvc;
using VisualVid.Web.Models.ViewModels;
using VisualVid.Web.Services;

namespace VisualVid.Web.Controllers;

public class BrowseController : Controller
{
    private readonly VideoService _videoService;
    private readonly MemberService _memberService;

    public BrowseController(VideoService videoService, MemberService memberService)
    {
        _videoService = videoService;
        _memberService = memberService;
    }

    public async Task<IActionResult> Index(int? categoryId, string? sort)
    {
        var categories = await _memberService.GetCategoriesAsync();
        var videos = categoryId.HasValue
            ? await _videoService.GetByCategoryAsync(categoryId.Value)
            : await _videoService.GetAllActiveAsync(sort);

        var model = new BrowseViewModel
        {
            Videos = videos,
            Categories = categories,
            SelectedCategoryId = categoryId,
            Sort = sort
        };

        return View(model);
    }
}
