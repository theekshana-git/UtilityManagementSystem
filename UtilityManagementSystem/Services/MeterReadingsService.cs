using Microsoft.EntityFrameworkCore;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services;

public class MeterReadingsService : IMeterReadingsService
{
    private readonly UtilityDbContext _context;

    public MeterReadingsService(UtilityDbContext context)
    {
        _context = context;
    }



    public async Task<IEnumerable<Meter>> GetAllMetersAsync()
    {
        return await _context.Meters
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<MeterReading>> GetReadingsHistoryAsync()
    {
        return await _context.MeterReadings
            .AsNoTracking()
            .Include(m => m.Meter)
            .Include(m => m.Meter.Customer)
            .OrderByDescending(m => m.ReadingDate)
            .ToListAsync();
    }

    public async Task<MeterReading?> GetLastReadingAsync(int meterId)
    {
        return await _context.MeterReadings
            .AsNoTracking()
            .Where(m => m.MeterId == meterId)
            .OrderByDescending(m => m.ReadingDate)
            .FirstOrDefaultAsync();
    }

    public async Task<MeterReading?> GetReadingByIdAsync(int readingId)
    {
        return await _context.MeterReadings
            .Include(m => m.Meter)
            .FirstOrDefaultAsync(m => m.ReadingId == readingId);
    }


    public async Task AddReadingAsync(MeterReadingViewModel model, int employeeId, string? autoNote = null)
    {
        var reading = new MeterReading
        {
            MeterId = model.MeterId,
            ReadingDate = DateOnly.FromDateTime(model.ReadingDate),
            CurrentReading = model.CurrentReading,
            RecordedBy = employeeId,
            PreviousReading = 0,
            Notes = autoNote ?? "Web Entry"
        };

        _context.MeterReadings.Add(reading);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateReadingAsync(MeterReading reading)
    {
        _context.MeterReadings.Update(reading);
        await _context.SaveChangesAsync();
    }



    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .AsNoTracking()
            .OrderBy(c => c.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Meter>> GetMetersByCustomerIdAsync(int customerId)
    {
        return await _context.Meters
            .AsNoTracking()
            .Include(m => m.Utility)
            .Where(m => m.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<dynamic> GetMeterFullDetailsAsync(int meterId)
    {

        var meter = await _context.Meters
            .AsNoTracking()
            .Include(m => m.Utility)
            .FirstOrDefaultAsync(m => m.MeterId == meterId);

        if (meter == null) return null;


        var lastReading = await _context.MeterReadings
            .AsNoTracking()
            .Where(mr => mr.MeterId == meterId)
            .OrderByDescending(mr => mr.ReadingDate)
            .Select(mr => mr.CurrentReading)
            .FirstOrDefaultAsync();

        return new
        {
            Location = meter.Location,
            Utility = meter.Utility?.UtilityName ?? "Unknown",
            Status = meter.Status,
            PreviousReading = lastReading
        };
    }
}