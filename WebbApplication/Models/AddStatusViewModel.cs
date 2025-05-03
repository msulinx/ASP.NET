using System.ComponentModel.DataAnnotations;

namespace WebbApplication.Models;

public class AddStatusViewModel
{
    [Required(ErrorMessage = "Status name is required.")]
    public string StatusName { get; set; } = null!;
}