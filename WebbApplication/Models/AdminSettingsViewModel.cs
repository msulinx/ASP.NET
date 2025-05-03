namespace WebbApplication.Models;

public class AdminSettingsViewModel
{
    public List<StatusViewModel> Statuses { get; set; } = new();
    
    public List<ClientViewModel> Clients { get; set; } = new();
    
    public AddStatusViewModel AddStatusFormData { get; set; } = new();
    public UpdateStatusViewModel UpdateStatusFormData { get; set; } = new();
    
    public AddClientViewModel AddClientFormData { get; set; } = new();
    public UpdateClientViewModel UpdateClientFormData { get; set; } = new();
}