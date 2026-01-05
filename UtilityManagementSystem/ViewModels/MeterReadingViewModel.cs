using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UtilityManagementSystem.ViewModels;

public class MeterReadingViewModel
{
    

    [Display(Name = "Select Customer")]
    [Required(ErrorMessage = "Please select a customer.")]
    public int CustomerId { get; set; }

    [Display(Name = "Select Meter")]
    [Required(ErrorMessage = "Please select a meter.")]
    public int MeterId { get; set; }

   

    [Display(Name = "Current Reading")]
    [Required(ErrorMessage = "Current reading is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Reading must be a positive number.")]
    public decimal CurrentReading { get; set; }

    [Display(Name = "Reading Date")]
    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date)]
    public DateTime ReadingDate { get; set; } = DateTime.Today;

   

    [Display(Name = "Previous Reading")]
    public decimal? PreviousReadingDisplay { get; set; }

    
    public IEnumerable<SelectListItem>? MeterOptions { get; set; }
}