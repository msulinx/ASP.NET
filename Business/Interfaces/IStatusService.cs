using Business.Results;
using Domain.FormData;

namespace Business.Interfaces;

public interface IStatusService
{
    Task<StatusResult> CreateStatusAsync(AddStatusFormData form);
    Task<StatusListResult> GetStatusesAsync();

    Task<StatusListResult> GetStatusesWithProjectAsync();

    Task<StatusResult> UpdateStatusAsync(int id, UpdateStatusFormData form);

    Task<StatusResult> DeleteStatusAsync(int id);
}