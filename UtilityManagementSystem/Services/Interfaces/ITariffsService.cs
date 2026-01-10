using System.Collections.Generic;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services.Interfaces
{
    public interface ITariffService
    {
        IEnumerable<TariffViewModel> GetCurrentTariffs();
        IEnumerable<TariffViewModel> GetTariffHistory(int utilityId);

        void AddTariff(TariffViewModel model);

        IEnumerable<UtilityType> GetUtilityTypes();
    }
}
