using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly UtilityDbContext _db;

        public CustomersService(UtilityDbContext db)
        {
            _db = db;
        }

        public List<CustomersViewModel> GetAllCustomers()
        {
            return _db.Customers.Select(c => new CustomersViewModel
            {
                CustomerID = c.CustomerId,
                Name = c.FirstName + " " + c.LastName,
                Type = c.CustomerType,
                City = c.City,
                Contact = c.ContactNumber,
                IsActive = c.Status == "Active",
                Outstanding = _db.Bills
                    .Where(b => b.CustomerId == c.CustomerId && b.Status == "Unpaid")
                    .Sum(b => (decimal?)b.TotalAmount) ?? 0
            }).ToList();
        }

        public CustomersViewModel GetCustomerById(int id)
        {
            return _db.Customers
                .Where(c => c.CustomerId == id)
                .Select(c => new CustomersViewModel
                {
                    CustomerID = c.CustomerId,
                    Name = c.FirstName + " " + c.LastName,
                    Type = c.CustomerType,
                    City = c.City,
                    Contact = c.ContactNumber,
                    IsActive = c.Status == "Active"
                }).FirstOrDefault();
        }

        public CustomersViewModel GetCustomerDetails(int customerId)
        {
            var customer = _db.Customers
                .Include(c => c.Meters)
                .Include(c => c.Bills)
                .Include(c => c.Payments)
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null) return null;

            var vm = new CustomersViewModel
            {
                CustomerID = customer.CustomerId,
                Name = customer.FirstName + " " + customer.LastName,
                Type = customer.CustomerType,
                City = customer.City,
                Contact = customer.ContactNumber,
                IsActive = customer.Status == "Active",
                Outstanding = customer.Bills
                    .Where(b => b.Status == "Unpaid")
                    .Sum(b => (decimal?)b.TotalAmount) ?? 0,

                Meters = customer.Meters.Select(m => new MeterViewModel
                {
                    MeterID = m.MeterId,
                    MeterNumber = m.MeterId.ToString(),
                    InstallationDate = m.InstallationDate.ToDateTime(TimeOnly.MinValue)
                }).ToList(),

                UnpaidBills = customer.Bills
                    .Where(b => b.Status == "Unpaid")
                    .Select(b => new BillViewModel
                    {
                        BillID = b.BillId,
                        Amount = b.TotalAmount,
                        DueDate = b.BillDate.Value.ToDateTime(TimeOnly.MinValue)
                    }).ToList(),

                Payments = customer.Payments.Select(p => new PaymentViewModel
                {
                    PaymentID = p.PaymentId,
                    Amount = p.AmountPaid,
                    Date = p.PaymentDate
                }).ToList()
            };

            CustomerUsageViewModel? usageVm = null;

            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "sp_CustomerUsageSummary";
                command.CommandType = CommandType.StoredProcedure;

                var param = new SqlParameter("@CustomerID", customerId);
                command.Parameters.Add(param);

                _db.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usageVm = new CustomerUsageViewModel
                        {
                            TotalUsage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                            AvgUsagePerMonth = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3)
                        };
                    }
                }

                _db.Database.CloseConnection();
            }

            vm.UsageSummary = usageVm;


            return vm;
        }

        public void AddCustomer(CustomersViewModel model)
        {
            var names = model.Name.Split(' ', 2);

            var customer = new Customer
            {
                FirstName = names[0],
                LastName = names.Length > 1 ? names[1] : "",
                Address = model.Address,
                City = model.City,
                ContactNumber = model.Contact,
                Email = model.Email,
                CustomerType = model.Type,
                Status = "Active",
                RegistrationDate = DateOnly.FromDateTime(model.Register)
            };

            _db.Customers.Add(customer);
            _db.SaveChanges();
        }


        public void UpdateCustomer(CustomerEditViewModel model)
        {
            var customer = _db.Customers.FirstOrDefault(c => c.CustomerId == model.CustomerID);
            if (customer == null) return;

            var names = model.Name.Split(' ', 2);
            customer.FirstName = names[0];
            customer.LastName = names.Length > 1 ? names[1] : "";

            customer.City = model.City;
            customer.ContactNumber = model.Contact;
            customer.CustomerType = model.Type;

            _db.SaveChanges();
        }


        public void ToggleActive(int id)
        {
            var customer = _db.Customers.FirstOrDefault(c => c.CustomerId == id);
            if (customer == null) return;

            customer.Status = customer.Status == "Active" ? "Inactive" : "Active";
            _db.SaveChanges();
        }
    }
}