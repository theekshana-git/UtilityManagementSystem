using UtilityManagementSystem.ViewModels;
using UtilityManagementSystem.Models; // Assumes your DB models are here

namespace UtilityManagementSystem.Services.Interfaces
{
    public interface IBillingService
    {
        // Get all bills with optional filters
        Task<IEnumerable<BillListViewModel>> GetAllBillsAsync(string status, string search);

        // Get only defaulters (Overdue/Unpaid) using Stored Procedure
        Task<IEnumerable<BillListViewModel>> GetDefaultersAsync();

        // Get single bill details
        Task<BillDetailsViewModel?> GetBillDetailsAsync(int billId);

        // Generate a new bill using Stored Procedure
        Task GenerateBillAsync(int readingId, int generatedByUserId);

        // Get list of readings that haven't been billed yet (for dropdown)
        Task<IEnumerable<dynamic>> GetUnbilledReadingsAsync();

        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<IEnumerable<Meter>> GetMetersByCustomerIdAsync(int customerId);
        Task<MeterReading?> GetLatestUnbilledReadingAsync(int meterId);
    }
}