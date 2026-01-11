using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentsService _paymentsService;

        public PaymentsController(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        public async Task<IActionResult> Index(string search, string method)
        {
            var payments = await _paymentsService.GetPaymentHistoryAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                payments = payments
                    .Where(p => p.Customer
                        .Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(method))
            {
                payments = payments
                    .Where(p => p.PaymentMethod == method)
                    .ToList();
            }

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentMethod = method;

            return View(payments);
        }


        public async Task<IActionResult> Create()
        {
            var unpaidBills = await _paymentsService.GetUnpaidBillsAsync();

            var vm = new PaymentsViewModel
            {
                UnpaidBills = unpaidBills
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.UnpaidBills = await _paymentsService.GetUnpaidBillsAsync();
                return View(model);
            }

            // Replace with logged-in user later
            int processedBy = 1;

            await _paymentsService.RecordPaymentAsync(
                billId: model.BillId,
                amountPaid: model.AmountPaid,
                paymentMethod: model.PaymentMethod, // ✅ this is a string now
                processedBy: processedBy
            );

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var payment = await _paymentsService.GetPaymentHistoryAsync();
            var selectedPayment = payment.FirstOrDefault(p => p.PaymentId == id);

            if (selectedPayment == null)
                return NotFound();

            return View(selectedPayment);
        }

    }
}
