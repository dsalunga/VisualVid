using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VisualVid.Core.Helpers;
using VisualVid.Web.Models;
using VisualVid.Web.Models.ViewModels;
using VisualVid.Web.Services;

namespace VisualVid.Web.Areas.Member.Controllers;

[Area("Member")]
[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MemberService _memberService;
    private readonly IWebHostEnvironment _environment;

    public ProfileController(
        UserManager<ApplicationUser> userManager,
        MemberService memberService,
        IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _memberService = memberService;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account", new { area = "" });

        var member = await _memberService.GetByUserIdAsync(user.Id);
        ViewBag.Countries = (await _memberService.GetCountriesAsync())
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(c.Name, c.CountryCode.ToString()));

        var model = new MemberProfileEditViewModel
        {
            UserName = user.UserName,
            PhotoUrl = $"/videos/members/{user.Id}.jpg",
            Email = user.Email
        };

        if (member != null)
        {
            model.FirstName = member.FirstName;
            model.LastName = member.LastName;
            model.CountryId = member.CountryCode;
            model.AboutMe = null; // No about-me in legacy schema
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MemberProfileEditViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        ViewBag.Countries = (await _memberService.GetCountriesAsync())
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(c.Name, c.CountryCode.ToString()));

        if (!ModelState.IsValid)
        {
            model.PhotoUrl = $"/videos/members/{user.Id}.jpg";
            return View(model);
        }

        await _memberService.CreateOrUpdateAsync(user.Id, model.CountryId ?? 0, false, DateTime.UtcNow);

        // Handle profile photo upload
        if (model.Photo != null && model.Photo.Length > 0)
        {
            // Validate image file type
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var photoExt = Path.GetExtension(model.Photo.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(photoExt) || !allowedImageExtensions.Contains(photoExt))
            {
                ModelState.AddModelError("Photo", "Invalid image file type. Allowed: JPG, PNG, GIF, BMP, WEBP.");
                model.PhotoUrl = $"/videos/members/{user.Id}.jpg";
                return View(model);
            }

            // Limit photo size to 5MB
            if (model.Photo.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("Photo", "Profile photo must be under 5MB.");
                model.PhotoUrl = $"/videos/members/{user.Id}.jpg";
                return View(model);
            }

            var memberImagePath = Path.Combine(_environment.WebRootPath, "videos", "members", $"{user.Id}.jpg");
            var memberDir = Path.GetDirectoryName(memberImagePath);
            if (!string.IsNullOrEmpty(memberDir) && !Directory.Exists(memberDir))
                Directory.CreateDirectory(memberDir);

            // Save temporary file
            var tempPath = Path.Combine(Path.GetTempPath(), $"vv_temp_{Guid.NewGuid()}{Path.GetExtension(model.Photo.FileName)}");
            try
            {
                await using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                // Resize and save as JPEG
                var (srcWidth, srcHeight) = ImageUtil.GetImageSize(tempPath);
                int width = 118;
                int height = (srcHeight * width) / srcWidth;
                ImageUtil.GenerateThumbnail(tempPath, memberImagePath, width, height);
            }
            finally
            {
                if (System.IO.File.Exists(tempPath))
                    System.IO.File.Delete(tempPath);
            }
        }

        model.PhotoUrl = $"/videos/members/{user.Id}.jpg";
        ViewBag.Message = "Update successful.";
        return View(model);
    }

    [HttpGet]
    public IActionResult Password()
    {
        return View(new ChangePasswordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Password(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            ViewBag.Message = "Password changed successfully.";
            return View(new ChangePasswordViewModel());
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }
}
