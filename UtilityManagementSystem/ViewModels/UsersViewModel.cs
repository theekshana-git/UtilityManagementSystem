using System.ComponentModel.DataAnnotations;

namespace UtilityManagementSystem.ViewModels
{
    public class UsersViewModel
    {
        public int EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        // Create only
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}