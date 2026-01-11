using System;

namespace UtilityManagementSystem.ViewModels
{
    public class ComplaintSummaryViewModel
    {
        public int ComplaintId { get; set; }
        public string CustomerName { get; set; } = null!;
        public int UtilityId { get; set; }
        public string UtilityName { get; set; }
        public string? Status { get; set; }
        public DateOnly? ComplaintDate { get; set; }
    }
}
