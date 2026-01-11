using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services.Interfaces
{
    public interface IComplaintsService
    {
        Task<IEnumerable<ComplaintSummaryViewModel>> GetComplaintsAsync(string? status, int? employeeId);
        Task CreateComplaintAsync(ComplaintViewModel model);
        Task<ComplaintViewModel?> GetComplaintByIdAsync(int id);
        Task AssignComplaintAsync(int complaintId, int employeeId);
        Task ResolveComplaintAsync(int complaintId, string resolutionNote);
    }
}
