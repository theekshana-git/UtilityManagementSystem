using Microsoft.EntityFrameworkCore;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;
using UtilityManagementSystem.Models;

namespace UtilityManagementSystem.Services
{
    public class BillingService : IBillingService
    {
        private readonly UtilityDbContext _context;

        public BillingService(UtilityDbContext context)
        {
            _context = context;
        }

        // --- NEW METHODS ADDED TO SOLVE CONTROLLER ERRORS ---

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.OrderBy(c => c.FirstName).ToListAsync();
        }

        public async Task<IEnumerable<Meter>> GetMetersByCustomerIdAsync(int customerId)
        {
            return await _context.Meters.Where(m => m.CustomerId == customerId).ToListAsync();
        }

        public async Task<MeterReading?> GetLatestUnbilledReadingAsync(int meterId)
        {
            var billedReadingIds = await _context.Bills.Select(b => b.ReadingId).ToListAsync();
            return await _context.MeterReadings
                .Where(r => r.MeterId == meterId && !billedReadingIds.Contains(r.ReadingId))
                .OrderByDescending(r => r.ReadingDate)
                .FirstOrDefaultAsync();
        }

        // --- EXISTING METHODS ---

        public async Task<IEnumerable<BillListViewModel>> GetAllBillsAsync(string status, string search)
        {
            var query = from b in _context.Bills
                        join c in _context.Customers on b.CustomerId equals c.CustomerId
                        select new { Bill = b, Customer = c };

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(x => x.Bill.Status == status);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Customer.FirstName.Contains(search) || x.Customer.LastName.Contains(search));
            }

            var dataFromDb = await query.ToListAsync();

            var result = dataFromDb.Select(x => new BillListViewModel
            {
                BillID = x.Bill.BillId,
                CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,
                TotalAmount = x.Bill.TotalAmount,
                BillDate = x.Bill.BillDate.ToDateTime(TimeOnly.MinValue),
                DueDate = x.Bill.DueDate.ToDateTime(TimeOnly.MinValue),
                Status = x.Bill.Status
            }).OrderByDescending(b => b.BillDate).ToList();

            return result;
        }

        public async Task<IEnumerable<BillListViewModel>> GetDefaultersAsync()
        {
            var defaulters = new List<BillListViewModel>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "sp_ListDefaulters";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                await _context.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new BillListViewModel();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string name = reader.GetName(i);
                            if (reader.IsDBNull(i)) continue;

                            // Match columns regardless of case (BillID vs BillId)
                            if (name.Equals("BillID", StringComparison.OrdinalIgnoreCase))
                                item.BillID = reader.GetInt32(i);
                            else if (name.Equals("CustomerName", StringComparison.OrdinalIgnoreCase))
                                item.CustomerName = reader.GetString(i);
                            else if (name.Equals("TotalAmount", StringComparison.OrdinalIgnoreCase))
                                item.TotalAmount = reader.GetDecimal(i);
                            else if (name.Equals("DueDate", StringComparison.OrdinalIgnoreCase))
                                item.DueDate = reader.GetDateTime(i);
                        }

                        item.Status = "Unpaid";
                        defaulters.Add(item);
                    }
                }
            }
            return defaulters;
        }

        public async Task<BillDetailsViewModel?> GetBillDetailsAsync(int billId)
        {
            var bill = await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.Reading)
                .FirstOrDefaultAsync(b => b.BillId == billId);

            if (bill == null) return null;

            // Pass DueDate and TotalAmount to match your SQL function signature
            var lateFee = await _context.Database
            .SqlQueryRaw<decimal>("SELECT dbo.udf_LateFee({0}, {1}) AS Value", bill.DueDate, bill.TotalAmount)
            .FirstOrDefaultAsync();

            return new BillDetailsViewModel
            {
                BillID = bill.BillId,
                CustomerName = bill.Customer.FirstName + " " + bill.Customer.LastName,
                Address = bill.Customer.Address,
                City = bill.Customer.City,
                ContactNumber = bill.Customer.ContactNumber,
                Email = bill.Customer.Email,
                ReadingID = bill.ReadingId,
                ReadingDate = bill.Reading.ReadingDate.ToDateTime(TimeOnly.MinValue),
                UnitsConsumed = bill.Reading.Consumption ?? 0,
                BillDate = bill.BillDate.ToDateTime(TimeOnly.MinValue),
                DueDate = bill.DueDate.ToDateTime(TimeOnly.MinValue),
                TotalAmount = bill.TotalAmount,
                Status = bill.Status,
                LateFee = lateFee // Pass the 5% calculation to the view
            };

        }

        public async Task GenerateBillAsync(int readingId, int generatedByUserId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_GenerateBill @p0, @p1", readingId, generatedByUserId);
        }

        public async Task<IEnumerable<dynamic>> GetUnbilledReadingsAsync()
        {
            var billedReadingIds = await _context.Bills.Select(b => b.ReadingId).ToListAsync();

            return await _context.MeterReadings
                .Where(r => !billedReadingIds.Contains(r.ReadingId))
                .Select(r => new
                {
                    ReadingID = r.ReadingId,
                    Description = "Reading #" + r.ReadingId + " - " + r.ReadingDate.ToString("yyyy-MM-dd") + " (" + r.Consumption + " Units)"
                })
                .ToListAsync();
        }
    }
}