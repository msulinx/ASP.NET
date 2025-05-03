using System.ComponentModel.DataAnnotations;

namespace WebbApplication.Models;

public class UserAddressViewModel
{
    [Required(ErrorMessage = "Street name is required.")]
    public string StreetName { get; set; } = null!;

    [Required(ErrorMessage = "Postal code is required.")]
    public string PostalCode { get; set; } = null!;

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; } = null!;
}