using System.ComponentModel.DataAnnotations;

namespace UtilityManagementSystem.ViewModels
{
    public class UsersEditViewModel
    {
        public int EmployeeId { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
