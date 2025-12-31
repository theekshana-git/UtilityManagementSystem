using System.Collections.Generic;
using System.Threading.Tasks;
using UtilityManagementSystem.Models;

namespace UtilityManagementSystem.Services.Interfaces
{
    public interface IMetersService
    {
        Task<IEnumerable<Meter>> GetAllMetersAsync();
        Task AddMeterAsync(Meter meter);
        Task DeleteMeterAsync(int id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<IEnumerable<UtilityType>> GetAllUtilitiesAsync();
    }
}
