using Domain.FormData;
using Domain.Models;
using WebbApplication.Models;

namespace WebbApplication.Extensions;

public static class StatusMappingExtensions
{
    // Status --> Viewmodel
    public static StatusViewModel MapToViewModel(this Status status)
    {
        return new StatusViewModel
        {
            Id = status.Id,
            StatusName = status.StatusName,
            ProjectCount = status.ProjectCount,
        };
    }
    
    // Viewmodel --> Formdata
    public static AddStatusFormData MapTo(this AddStatusViewModel model)
    {
        return new AddStatusFormData
        {
            StatusName = model.StatusName,
        };
    }

    // Viewmodel --> Formdata
    public static UpdateStatusFormData MapTo(this UpdateStatusViewModel model)
    {
        return new UpdateStatusFormData
        {
            Id = model.Id,
            StatusName = model.StatusName
        };
    }

}