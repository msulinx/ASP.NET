using System.ComponentModel.DataAnnotations;

namespace WebbApplication.Models;

public class AddClientViewModel
{
    
    [Required(ErrorMessage = "Client name is required")]
    public string ClientName { get; set; } = null!;
    
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;
}