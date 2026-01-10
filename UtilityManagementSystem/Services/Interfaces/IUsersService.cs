using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services.Interfaces
{
    public interface IUsersService
    {
        IEnumerable<UsersViewModel> GetAll();
        UsersViewModel GetById(int id);
        void Create(UsersViewModel model);
        void UpdateRoleStatus(UsersEditViewModel model);

    }
}
