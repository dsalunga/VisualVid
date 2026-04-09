using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VisualVid.Web.Models;
using VisualVid.Web.Models.ViewModels;
using VisualVid.Web.Services;

namespace VisualVid.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly MemberService _memberService;
    private readonly EmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        MemberService memberService,
        EmailService emailService,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _memberService = memberService;
        _emailService = emailService;
        _configuration = configuration;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.IsInRoleAsync(user, "Administrators"))
                return RedirectToAction("Index", "Admin", new { area = "Admin" });

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Dashboard", new { area = "Member" });
        }

        if (result.IsLockedOut)
            ModelState.AddModelError(string.Empty, "Account locked out. Please try again later.");
        else
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        ViewBag.Countries = (await _memberService.GetCountriesAsync())
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(c.Name, c.CountryCode.ToString()));
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ViewBag.Countries = (await _memberService.GetCountriesAsync())
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(c.Name, c.CountryCode.ToString()));

        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = model.UserName,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            // Ensure Members role exists
            if (!await _roleManager.RoleExistsAsync("Members"))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = "Members"
                });
            }

            await _userManager.AddToRoleAsync(user, "Members");

            // Create member profile
            await _memberService.CreateOrUpdateAsync(user.Id, model.CountryId ?? 0, false, DateTime.UtcNow);

            // Copy default profile image
            var defaultImage = Path.Combine(_environment.WebRootPath, "images", "img_userpic.jpg");
            var memberImage = Path.Combine(_environment.WebRootPath, "videos", "members", $"{user.Id}.jpg");
            var memberDir = Path.GetDirectoryName(memberImage);
            if (!string.IsNullOrEmpty(memberDir) && !Directory.Exists(memberDir))
                Directory.CreateDirectory(memberDir);
            if (System.IO.File.Exists(defaultImage))
                System.IO.File.Copy(defaultImage, memberImage, overwrite: true);

            // Send confirmation email
            try
            {
                var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmUrl = Url.Action("Confirm", "Account",
                    new { userId = user.Id, token = confirmToken }, Request.Scheme);
                var body = $"<span style=\"font-family:Tahoma;font-size:10pt\">Username: {model.UserName}<br />" +
                           $"<br /><a href=\"{confirmUrl}\">Click here to activate your account</a></span>";
                await _emailService.SendAsync(model.Email, "Your VisualVid Registration", body);
            }
            catch
            {
                // Email failure should not block registration
            }

            return RedirectToAction(nameof(RegisterComplete));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    [HttpGet]
    public IActionResult RegisterComplete() => View();

    [HttpGet]
    public async Task<IActionResult> Confirm(string? userId, string? token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            return RedirectToAction("Index", "Home");

        var appUser = await _userManager.FindByIdAsync(userId);
        if (appUser == null)
        {
            ViewBag.Status = "failed";
            return View();
        }

        if (appUser.EmailConfirmed)
        {
            ViewBag.Status = "already";
            return View();
        }

        var result = await _userManager.ConfirmEmailAsync(appUser, token);
        ViewBag.Status = result.Succeeded ? "success" : "failed";

        return View();
    }

    [HttpGet]
    public IActionResult ForgotPassword() => View(new ForgotPasswordViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = Url.Action("ResetPassword", "Account",
                new { token, email = user.Email }, Request.Scheme);

            try
            {
                await _emailService.SendAsync(user.Email!,
                    "VisualVid Password Reset",
                    $"<p>Click <a href=\"{resetUrl}\">here</a> to reset your password.</p>");
            }
            catch { }
        }

        ViewBag.Message = "If the account exists, a reset link has been sent.";
        return View(model);
    }

    [HttpGet]
    public IActionResult ChangePassword() => View(new ChangePasswordViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction(nameof(Login));

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            await _signInManager.RefreshSignInAsync(user);
            ViewBag.Message = "Password changed successfully.";
            return View(new ChangePasswordViewModel());
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    public IActionResult AccessDenied() => View();
}
