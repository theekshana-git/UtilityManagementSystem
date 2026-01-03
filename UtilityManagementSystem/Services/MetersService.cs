using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;

namespace UtilityManagementSystem.Services
{
    public class MetersService : IMetersService
    {
        private readonly UtilityDbContext _context;

        public MetersService(UtilityDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Meter>> GetAllMetersAsync()
        {
            return await _context.Meters
                .Include(m => m.Customer)
                .Include(m => m.Utility)
                .ToListAsync();
        }

        public async Task AddMeterAsync(Meter meter)
        {
            _context.Meters.Add(meter);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMeterAsync(int id)
        {
            var meter = await _context.Meters.FindAsync(id);
            if (meter != null)
            {
                _context.Meters.Remove(meter);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<IEnumerable<UtilityType>> GetAllUtilitiesAsync()
        {
            return await _context.UtilityTypes.ToListAsync();
        }
    }
}