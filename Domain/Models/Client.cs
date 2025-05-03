namespace Domain.Models;

public class Client
{
    public int Id { get; set; } 
    
    public string ClientName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
}