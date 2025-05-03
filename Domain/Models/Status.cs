namespace Domain.Models;

public class Status
{
    public int Id { get; set; }
    
    public string StatusName { get; set; } = null!;
    
    public int ProjectCount { get; set; }
}