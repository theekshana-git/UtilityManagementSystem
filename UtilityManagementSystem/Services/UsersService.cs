using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;
using BCrypt.Net;
using Microsoft.CodeAnalysis.Scripting;

namespace UtilityManagementSystem.Services
{
    public class UsersService : IUsersService
    {
        private readonly UtilityDbContext _context;

        public UsersService(UtilityDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UsersViewModel> GetAll()
        {
            return _context.Employees
                .Select(e => new UsersViewModel
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Username = e.Username,
                    Role = e.Role,
                    Status = e.Status,
                    ContactNumber = e.ContactNumber,
                    Email = e.Email
                }).ToList();
        }

        public UsersViewModel GetById(int id)
        {
            var e = _context.Employees.Find(id);

            return new UsersViewModel
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Username = e.Username,
                Role = e.Role,
                Status = e.Status,
                ContactNumber = e.ContactNumber,
                Email = e.Email
            };
        }

        public void Create(UsersViewModel model)
        {
            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Role = model.Role,
                Status = "Active",
                ContactNumber = model.ContactNumber,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void UpdateRoleStatus(UsersEditViewModel model)
        {
            var emp = _context.Employees.Find(model.EmployeeId);
            if (emp == null) return;

            emp.Role = model.Role;
            emp.Status = model.Status;

            _context.SaveChanges();
        }
    }
}
