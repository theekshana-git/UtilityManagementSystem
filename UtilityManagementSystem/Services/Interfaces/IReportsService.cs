using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services.Interfaces
{
    public interface IReportsService
    {
        Task<ReportViewModel> GetDashboardDataAsync();
        Task<List<DefaulterRow>> GetDefaultersAsync();
        Task<CustomerUsageResult?> GetCustomerUsageSummaryAsync(int customerId);
        Task<decimal> GetMonthlyRevenueAsync(int year, int month);
    }
}