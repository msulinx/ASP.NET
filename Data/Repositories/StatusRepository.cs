using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class StatusRepository(AppDbContext context) 
    : BaseRepository<StatusEntity, Status>(context), IStatusRepository
{
    private readonly AppDbContext _context = context;


    public async Task<List<Status>> GetAllStatusesWithProjectsAsync()
    {
        var result = await _context.Statuses
            .Select(status => new Status
            {
                Id = status.Id,
                StatusName = status.StatusName,
                ProjectCount = _context.Projects.Count(p => p.StatusId == status.Id)
            }).ToListAsync();
        
        return result;
    }
}