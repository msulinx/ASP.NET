using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebbApplication.Models;

namespace WebbApplication.Controllers;

public class CookiesController : Controller
{
    
    [HttpPost]
    public IActionResult SetCookies([FromBody] CookieConsent consent)
    {
        Response.Cookies.Append("SessionsCookie", "Essential", new CookieOptions
        {
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddYears(1),
        });
        
        if (consent == null)
            return BadRequest();

        if (consent.Functional)
        {
            Response.Cookies.Append("FunctionalCookie", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }
        else
        {
            Response.Cookies.Delete("FunctionalCookie");
        }
        
        if (consent.Analytics)
        {
            Response.Cookies.Append("AnalyticsCookie", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }
        else
        {
            Response.Cookies.Delete("AnalyticsCookie");
        }
        
        if (consent.Marketing)
        {
            Response.Cookies.Append("MarketingCookie", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }
        else
        {
            Response.Cookies.Delete("MarketingCookie");
        }
        
        Response.Cookies.Append("cookieConsent", JsonSerializer.Serialize(consent), new CookieOptions
        {
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(90),
            SameSite = SameSiteMode.Lax,
            Path = "/",
            HttpOnly = false 
            
        });
        
        return Ok();
    }
}