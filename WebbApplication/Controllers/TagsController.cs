using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebbApplication.Controllers;

public class TagsController(AppDbContext context) : Controller
{
    private readonly AppDbContext _context = context;
    
    public async Task<IActionResult> SearchMembers(string term)
    {
        var roleId = _context.Roles.FirstOrDefault(r => r.Name == "Member")?.Id;

        if (roleId == null)
            return Json(new List<object>());

        var query = _context.Users
            .Where(u => _context.UserRoles.Any(r => r.UserId == u.Id && r.RoleId == roleId));

        if (!string.IsNullOrWhiteSpace(term))
        {
            query = query.Where(x =>
                x.FirstName!.Contains(term) ||
                x.LastName!.Contains(term));
        }

        var result = await query
            .Select(user => new
            {
                id = user.Id,
                fullName = user.FirstName + " " + user.LastName,
                imageUrl = user.UserImage
            })
            .ToListAsync();
        
        return Json(result);
    }
    
    public async Task<IActionResult> SearchClients(string term)
    {
        var query = _context.Clients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(term))
            query = query.Where(x => x.ClientName.Contains(term));

        var tags = await query
            .Select(x => new
            {
                x.Id,
                x.ClientName
            })
            .ToListAsync();

        return Json(tags);
    }

    public async Task<IActionResult> SearchStatus(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        var tags = await _context.Statuses
            .Where(x => x.StatusName.Contains(term))
            .Select(x => new
            {
                x.Id,
                x.StatusName
            })
            .ToListAsync();

        return Json(tags);
    }
}