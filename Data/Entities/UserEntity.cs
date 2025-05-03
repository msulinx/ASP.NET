using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class UserEntity : IdentityUser
{
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    
    public string? JobTitle { get; set; }
    
    public string? UserImage { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    public UserAddressEntity Address { get; set; } = new();

    public virtual ICollection<ProjectMemberEntity> ProjectMembers { get; set; } = [];
    
    public ICollection<NotificationDismissEntity> NotificationDismisses { get; set; } = [];
}