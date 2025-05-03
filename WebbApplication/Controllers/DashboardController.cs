
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebbApplication.Controllers;

[Authorize]
[Route("dashboard")]
public class DashboardController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}