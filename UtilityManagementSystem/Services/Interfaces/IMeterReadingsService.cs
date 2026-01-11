using UtilityManagementSystem.Models;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services.Interfaces;

public interface IMeterReadingsService
{

    Task<IEnumerable<MeterReading>> GetReadingsHistoryAsync();
    Task<MeterReading?> GetReadingByIdAsync(int readingId);
    Task<MeterReading?> GetLastReadingAsync(int meterId);


    Task AddReadingAsync(MeterReadingViewModel model, int employeeId, string? autoNote = null);

    Task UpdateReadingAsync(MeterReading reading);


    Task<IEnumerable<Customer>> GetAllCustomersAsync();

    Task<IEnumerable<Meter>> GetAllMetersAsync();

    Task<IEnumerable<Meter>> GetMetersByCustomerIdAsync(int customerId);


    Task<dynamic> GetMeterFullDetailsAsync(int meterId);
}