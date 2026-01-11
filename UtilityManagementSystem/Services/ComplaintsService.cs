using Microsoft.EntityFrameworkCore;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services
{
    public class ComplaintsService : IComplaintsService
    {
        private readonly UtilityDbContext _context;

        public ComplaintsService(UtilityDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComplaintSummaryViewModel>> GetComplaintsAsync(string? status, int? employeeId)
        {
            var query = _context.VwComplaintsSummaries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status == status);

            // ❗ employee filter CANNOT be done here
            // because vw_ComplaintsSummary does not expose HandledBy

            return await query
                .OrderByDescending(c => c.ComplaintDate)
                .Select(c => new ComplaintSummaryViewModel
                {
                    ComplaintId = c.ComplaintId,
                    CustomerName = c.CustomerName,
                    UtilityId = c.UtilityId,
                    Status = c.Status,
                    ComplaintDate = c.ComplaintDate
                })
                .ToListAsync();
        }


        public async Task CreateComplaintAsync(ComplaintViewModel model)
        {
            var complaint = new Complaint
            {
                CustomerId = model.CustomerId,
                UtilityId = model.UtilityId,
                Description = model.Description,
                ComplaintDate = DateOnly.FromDateTime(DateTime.Now),
                Status = model.Status
            };

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();
        }

        public async Task<ComplaintViewModel?> GetComplaintByIdAsync(int id)
        {
            var complaint = await _context.Complaints
                .Include(c => c.Customer)
                .Include(c => c.Utility)
                .Include(c => c.HandledByNavigation)
                .FirstOrDefaultAsync(c => c.ComplaintId == id);

            if (complaint == null) return null;

            return new ComplaintViewModel
            {
                ComplaintId = complaint.ComplaintId,
                CustomerId = complaint.CustomerId,
                UtilityId = complaint.UtilityId,
                Description = complaint.Description,
                Status = complaint.Status!,
                HandledBy = complaint.HandledBy,
                ResolutionNote = complaint.ResolutionNote,
                ComplaintDate = complaint.ComplaintDate,
                ResolutionDate = complaint.ResolutionDate
            };
        }

        public async Task AssignComplaintAsync(int complaintId, int employeeId)
        {
            var complaint = await _context.Complaints.FindAsync(complaintId);
            if (complaint == null) return;

            complaint.HandledBy = employeeId;
            complaint.Status = "In Progress";

            await _context.SaveChangesAsync();
        }

        public async Task ResolveComplaintAsync(int complaintId, string resolutionNote)
        {
            var complaint = await _context.Complaints.FindAsync(complaintId);
            if (complaint == null) return;

            complaint.Status = "Resolved";
            complaint.ResolutionNote = resolutionNote;
            complaint.ResolutionDate = DateOnly.FromDateTime(DateTime.Now);

            await _context.SaveChangesAsync();
        }
    }
}
