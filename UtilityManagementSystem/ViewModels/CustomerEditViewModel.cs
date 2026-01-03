using System.ComponentModel.DataAnnotations;

namespace UtilityManagementSystem.ViewModels
{
    public class CustomerEditViewModel
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Customer Type is required")]
        public string Type { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Invalid contact number")]
        public string Contact { get; set; }
    }
}