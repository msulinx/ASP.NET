using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key] 
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? ProjectImage { get; set; }

    [Required] [MaxLength(100)] 
    public string ProjectName { get; set; } = null!;

    [MaxLength(1000)] 
    public string? Description { get; set; }

    [Column(TypeName = "date")] 
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "date")] 
    public DateTime? EndDate { get; set; }

    [Column(TypeName = "decimal(18,2)")] 
    public decimal? Budget { get; set; }

    public int ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;

    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.Now;

    public virtual ICollection<ProjectMemberEntity> ProjectMembers { get; set; } = [];
}