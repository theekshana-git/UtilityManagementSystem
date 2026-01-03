using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace UtilityManagementSystem.ViewModels
{
    public class MetersViewModel
    {
        public int MeterId { get; set; }

        public int CustomerId { get; set; }

        public int UtilityId { get; set; }

        public string Location { get; set; } = string.Empty;

        public string Status { get; set; } = "Active";

        public List<SelectListItem> CustomerList { get; set; } = new();

        public List<SelectListItem> UtilityList { get; set; } = new();
    }
}