using System.Collections.Generic;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.ViewModels;

namespace UtilityManagementSystem.Services.Interfaces
{
    public interface ITariffsService
    {
        // 🔹 Index (ONLY latest tariff per slab)
        IEnumerable<TariffViewModel> GetLatestTariffsPerSlab();

        // 🔹 History (slab-specific)
        IEnumerable<TariffViewModel> GetTariffHistoryBySlab(
    int utilityId,
    decimal slabStart,
    decimal? slabEnd
);


        // 🔹 Create
        void AddTariff(TariffViewModel model);

        // 🔹 Utilities
        IEnumerable<UtilityType> GetUtilityTypes();
    }
}
