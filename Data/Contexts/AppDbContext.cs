using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<UserEntity>(options)
{
    public virtual DbSet<ProjectEntity> Projects { get; set; }

    public virtual DbSet<ProjectMemberEntity> ProjectMembers { get; set; }

    public virtual DbSet<UserEntity> Members { get; set; }

    public virtual DbSet<StatusEntity> Statuses { get; set; }
    public virtual DbSet<ClientEntity> Clients { get; set; }
    
    public DbSet<NotificationEntity> Notifications { get; set; }
    
    public DbSet<NotificationTargetGroupEntity> NotificationTargetGroups { get; set; }
    
    public DbSet<NotificationTypeEntity> NotificationTypes { get; set; }
    
    public DbSet<NotificationDismissEntity> NotificationDismisses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<UserEntity>().OwnsOne(u => u.Address);

        builder.Entity<ProjectMemberEntity>()
            .HasKey(pm => new { pm.ProjectId, pm.UserId }); 

        builder.Entity<ProjectMemberEntity>()
            .HasOne(pm => pm.Project)
            .WithMany(p => p.ProjectMembers)
            .HasForeignKey(pm => pm.ProjectId);

        builder.Entity<ProjectMemberEntity>()
            .HasOne(pm => pm.Member)
            .WithMany(u => u.ProjectMembers)
            .HasForeignKey(pm => pm.UserId);
    }
}