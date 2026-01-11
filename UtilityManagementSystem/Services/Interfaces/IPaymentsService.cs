using System.Collections.Generic;
using System.Threading.Tasks;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services.Interfaces
{
    public interface IPaymentsService
    {
        Task<IEnumerable<VwUnpaidBill>> GetUnpaidBillsAsync();



        Task RecordPaymentAsync(
            int billId,
            decimal amountPaid,
            string paymentMethod,
            int processedBy
        );



        Task<IEnumerable<PaymentHistoryViewModel>> GetPaymentHistoryAsync();
    }
}
