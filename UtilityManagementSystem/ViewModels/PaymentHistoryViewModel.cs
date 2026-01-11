namespace UtilityManagementSystem.ViewModels
{
    public class PaymentHistoryViewModel
    {
        public int PaymentId { get; set; }
        public string Customer { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public int BillId { get; set; }
    }
}
