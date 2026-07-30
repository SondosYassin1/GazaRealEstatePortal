using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using GazaRealEstatePortal.Services.Interfaces;
using GazaRealEstatePortal.ViewModels;

namespace GazaRealEstatePortal.Controllers;

public class AuthController : Controller
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToDashboard(User.FindFirst(ClaimTypes.Role)?.Value);
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var registerInput = new GazaRealEstatePortal.ViewModels.RegisterInput
            {
                FullName = $"{model.FirstName} {model.LastName}",
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber
            };

            var user = await _userService.RegisterAsync(registerInput);

            // Sign in automatically
            await SignInUserAsync(user.Id.ToString(), user.Email, user.Role.ToString(), false);

            return Redirect("/Properties/Dashboard");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToDashboard(User.FindFirst(ClaimTypes.Role)?.Value);
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        GazaRealEstatePortal.Models.User? user = null;
        try
        {
            user = await _userService.AuthenticateAsync(model.Email, model.Password);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            return View(model);
        }

        await SignInUserAsync(user.Id.ToString(), user.Email, user.Role.ToString(), model.RememberMe);

        if (user.Role.ToString() == "Admin")
        {
            return Redirect("/Admin/Dashboard");
        }
        else
        {
            return Redirect("/Properties/Dashboard");
        }
    }

    [HttpGet]
    public IActionResult ExternalLogin(string provider)
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback") };
        return Challenge(properties, provider);
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            return RedirectToAction("Login");
        }

        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        if (claims == null)
        {
            return RedirectToAction("Login");
        }

        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var nameIdentifier = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        var provider = result.Principal.Identities.FirstOrDefault()?.AuthenticationType;

        if (string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError(string.Empty, "تعذر قراءة البريد الإلكتروني من مزود الخدمة.");
            return View("Login");
        }

        var user = await _userService.AuthenticateExternalAsync(email, provider ?? "External", nameIdentifier ?? "", name ?? "");

        await SignInUserAsync(user.Id.ToString(), user.Email, user.Role.ToString(), false);

        if (user.Role.ToString() == "Admin")
        {
            return Redirect("/Admin/Dashboard");
        }
        else
        {
            return Redirect("/Properties/Dashboard");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task SignInUserAsync(string userId, string email, string role, bool rememberMe)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectToDashboard(string? role)
    {
        if (role == "Admin")
        {
            return Redirect("/Admin/Dashboard");
        }
        return Redirect("/Properties/Dashboard");
    }
}
