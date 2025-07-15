using System.Security.Claims;
using Library.Models.Auth;
using Library.Models.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers;

public class AccountController(IAccountService service, IConfiguration config) : Controller
{
    private readonly IAccountService _service = service;
    private readonly IConfiguration _config = config;

    private void LoadGoogleClientId()
    {
        ViewBag.GoogleClientId = _config["OAuth:ClientId"];
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoadGoogleClientId();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        LoadGoogleClientId();

        if (!ModelState.IsValid)
            return View(model);

        var auth = await _service.LoginAsync(model);
        if (auth?.User == null)
        {
            TempData["Error"] = "Incorrect Email or Password.";
            return View(model);
        }

        await _service.SignInUser(auth.User, auth.Token);
        TempData["Success"] = "Logged in successfully.";
        return RedirectToAction("Index", "Announcement");
    }


    [HttpGet]
    public IActionResult Register()
    {
        var isRedirected = TempData["OAuthUser.IsRedirected"];
        var fullName = TempData["OAuthUser.FullName"];
        var email = TempData["OAuthUser.Email"];

        if (isRedirected != null && (bool)isRedirected && (fullName == null || email == null))
            TempData["Error"] = "Failed to load your data.";

        var model = new RegisterViewModel
        {
            FullName = fullName?.ToString()!,
            Email = email?.ToString()!
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var statusCode = await _service.RegisterAsync(model);

        if (!statusCode)
        {
            TempData["Error"] = "Registration failed.";
            return View(model);
        }

        TempData["Success"] = "Registered successfully! You can now log in.";
        return RedirectToAction("Login");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OAuthLogin(string token)
    {
        var (auth, fallback) = await _service.OAuthLoginAsync(token);

        if (auth != null)
        {
            await _service.SignInUser(auth.User, auth.Token);
            TempData["Success"] = "Logged in with Google.";
            return RedirectToAction("Index", "Announcement");
        }

        if (fallback != null)
        {
            TempData["OAuthUser.IsRedirected"] = true;
            TempData["OAuthUser.FullName"] = fallback.FullName;
            TempData["OAuthUser.Email"] = fallback.Email;
            return RedirectToAction("Register");
        }

        TempData["Error"] = "Failed to login via Google, please try again or do it manually.";
        return RedirectToAction("Login");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        Response.Cookies.Delete("AccessToken");

        TempData["LoggedOut"] = true;
        TempData["Success"] = "Logged out.";
        return RedirectToAction("Index", "Announcement");
    }
}