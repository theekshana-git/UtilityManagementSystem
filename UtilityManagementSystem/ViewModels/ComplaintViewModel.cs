using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace UtilityManagementSystem.ViewModels
{
    public class ComplaintViewModel
    {
        public int ComplaintId { get; set; }

        public int CustomerId { get; set; }
        public int UtilityId { get; set; }

        public string Description { get; set; } = null!;

        public string Status { get; set; } = "Pending";

        public int? HandledBy { get; set; }

        public string? ResolutionNote { get; set; }

        public DateOnly? ComplaintDate { get; set; }
        public DateOnly? ResolutionDate { get; set; }

        // Dropdowns
        public SelectList? Customers { get; set; }
        public SelectList? Utilities { get; set; }
        public SelectList? Employees { get; set; }
    }
}
