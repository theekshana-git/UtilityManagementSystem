using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly UtilityDbContext _context;

        public PaymentsService(UtilityDbContext context)
        {
            _context = context;
        }

        // ✅ MATCHES INTERFACE EXACTLY
        public async Task<IEnumerable<VwUnpaidBill>> GetUnpaidBillsAsync()
        {
            return await _context.VwUnpaidBills
                .OrderBy(b => b.DueDate)
                .ToListAsync();
        }

        // ✅ UPDATED SIGNATURE (NO customerId)
        public async Task RecordPaymentAsync(
            int billId,
            decimal amountPaid,
            string paymentMethod,
            int processedBy)
        {
            var bill = await _context.Bills
                .FirstOrDefaultAsync(b => b.BillId == billId);

            if (bill == null)
                throw new Exception("Bill not found.");

            if (amountPaid <= 0)
                throw new Exception("Invalid payment amount.");

            if (amountPaid > bill.TotalAmount)
                throw new Exception("Payment exceeds outstanding balance.");

            // Call stored procedure
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_RecordPayment @BillID={0}, @Amount={1}, @PaymentMethod={2}, @ProcessedBy={3}",
                billId, amountPaid, paymentMethod, processedBy
            );

            // Reload bill after DB logic
            await _context.Entry(bill).ReloadAsync();

            // Update status safely
            bill.Status = bill.TotalAmount == 0
                ? "Paid"
                : "Partially Paid";

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PaymentHistoryViewModel>> GetPaymentHistoryAsync()
        {
            return await _context.Payments
                .Include(p => p.Customer)
                .OrderByDescending(p => p.PaymentDate)
                .Select(p => new PaymentHistoryViewModel
                {
                    PaymentId = p.PaymentId,
                    Customer = p.Customer.FirstName + " " + p.Customer.LastName,
                    AmountPaid = p.AmountPaid,
                    PaymentDate = p.PaymentDate,
                    PaymentMethod = p.PaymentMethod!,
                    BillId = p.BillId
                })
                .ToListAsync();
        }
    }
}
