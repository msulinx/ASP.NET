using System.Security.Claims;
using Data.Entities;
using Domain.Extensions;
using Domain.FormData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebbApplication.Extensions;
using WebbApplication.Models;
using IAuthenticationService = Business.Interfaces.IAuthenticationService;

namespace WebbApplication.Controllers;

[AllowAnonymous]
public class AuthController(
    IAuthenticationService authenticationService,
    SignInManager<UserEntity> signInManager,
    UserManager<UserEntity> userManager) : Controller
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;

    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignUp()
    {
        var model = new SignUpViewModel
        {
            Roles = new List<SelectListItem>
            {
                new() { Text = "User", Value = "User" },
                new() { Text = "Admin", Value = "Admin" }
            }
        };
        
        return View(model);
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        model.Roles = new List<SelectListItem>
        {
            new() { Text = "User", Value = "User" },
            new() { Text = "Admin", Value = "Admin" }
        };

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var formData = model.MapTo();
        var result = await _authenticationService.SignUpAsync(formData);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to create account");
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(formData.Email);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, model.Role);
        }
        
        return RedirectToAction("Index", "Dashboard");
    }
    
    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email!);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "No account found with this email.");
            return View(model);
        }

        var formData = model.MapTo<SignInFormData>();
        var result = await _authenticationService.SignInAsync(formData);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Failed to sign in.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? ""),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.GivenName, user.FirstName ?? ""),
            new(ClaimTypes.Surname, user.LastName ?? ""),
            new("UserImage", user.UserImage ?? "/images/avatar.svg")
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            IdentityConstants.ApplicationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = formData.RememberMe });
        
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    public IActionResult ExternalSignIn(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("SignIn");
        }
        
        var redirectUrl = Url.Action("ExternalSignInCallback", "Auth", new { returnUrl })!;
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    public async Task<IActionResult> ExternalSignInCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Action("Index", "Dashboard");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"External login error: {remoteError}");
            return View("SignIn");
        }
        
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("SignIn");
        
        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (signInResult.Succeeded)
            return LocalRedirect(returnUrl);
        
        else
        {
            string firstName = string.Empty;
            string lastName = string.Empty;

            try
            {
                firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!;
                lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!;
            }
            catch { }
            string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            string userName = $"ext_{info.LoginProvider.ToLower()}_{email}";
            
            var user = new UserEntity { UserName = userName, Email = email, FirstName = firstName, LastName = lastName };
            var identityRole = await _userManager.CreateAsync(user);
            if (identityRole.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in identityRole.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return View("SignIn");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
}