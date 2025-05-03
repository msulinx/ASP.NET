using System.Linq.Expressions;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository : BaseRepository<ProjectEntity, Project>, IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectEntity>> GetProjectsWithDetailsAsync()
    {
        return await _context.Projects
            .Include(p => p.Client)
            .Include(p => p.Status)
            .Include(p => p.ProjectMembers)
            .ThenInclude(pm => pm.Member)
            .ToListAsync();
    }
}