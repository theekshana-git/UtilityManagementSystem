using System.Collections.Generic;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services.Interfaces
{
    public interface ICustomersService
    {
        List<CustomersViewModel> GetAllCustomers();
        CustomersViewModel GetCustomerById(int id);
        CustomersViewModel GetCustomerDetails(int id);

        void AddCustomer(CustomersViewModel model);
        void UpdateCustomer(CustomerEditViewModel model);
        void ToggleActive(int id);
    }
}
