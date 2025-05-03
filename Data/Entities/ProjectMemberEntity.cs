using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectMemberEntity
{
    [ForeignKey(nameof(Member))]
    public string UserId { get; set; }
    
    public UserEntity Member { get; set; }

    [ForeignKey(nameof(Project))]
    public string? ProjectId { get; set; }
    
    public ProjectEntity Project { get; set; }
}