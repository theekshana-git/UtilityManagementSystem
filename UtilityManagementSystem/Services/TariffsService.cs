using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

public class TariffsService : ITariffsService
{
    private readonly UtilityDbContext _context;

    public TariffsService(UtilityDbContext context)
    {
        _context = context;
    }

    public IEnumerable<UtilityType> GetUtilityTypes()
    {
        return _context.UtilityTypes.ToList();
    }


    //Add Tarrif
    public void AddTariff(TariffViewModel model)
    {
        var effectiveFrom = DateOnly.FromDateTime(model.EffectiveFrom);

        // 1️⃣ Expire existing active tariffs (same Utility + Slab)
        var activeTariffs = _context.Tariffs
            .Where(t =>
                t.UtilityId == model.UtilityId &&
                t.SlabStart == model.SlabStart &&
                t.SlabEnd == model.SlabEnd &&
                t.EffectiveTo == null)
            .ToList();

        foreach (var old in activeTariffs)
        {
            old.EffectiveTo = effectiveFrom.AddDays(-1);

            _context.UtilityTariffHistories.Add(new UtilityTariffHistory
            {
                UtilityId = old.UtilityId,
                TariffId = old.TariffId,
                ChangedBy = 1, // TEMP: replace with logged-in employee ID
                ChangedOn = DateOnly.FromDateTime(DateTime.Now)
            });
        }

        // 2️⃣ Add new tariff
        _context.Tariffs.Add(new Tariff
        {
            UtilityId = model.UtilityId!.Value,
            SlabStart = model.SlabStart!.Value,
            SlabEnd = model.SlabEnd,
            RatePerUnit = model.RatePerUnit,
            FixedCharge = model.FixedCharge,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = null
        });

        _context.SaveChanges();
    }

    //current tariffs
    public IEnumerable<TariffViewModel> GetCurrentTariffs()
    {
        return _context.Tariffs
            .Where(t => t.EffectiveTo == null)
            .OrderByDescending(t => t.EffectiveFrom)
            .Select(t => new TariffViewModel
            {
                TariffId = t.TariffId,
                UtilityId = t.UtilityId,
                UtilityName = t.Utility.UtilityName,
                SlabStart = t.SlabStart,
                SlabEnd = t.SlabEnd,
                RatePerUnit = t.RatePerUnit,
                FixedCharge = t.FixedCharge,
                EffectiveFrom = t.EffectiveFrom.ToDateTime(TimeOnly.MinValue)
            })
            .ToList();
    }

  //tariff history
    public IEnumerable<TariffViewModel> GetTariffHistory(int utilityId)
    {
        return _context.Tariffs
            .Where(t => t.UtilityId == utilityId && t.EffectiveTo != null)
            .OrderByDescending(h => h.EffectiveFrom)
            .Select(t => new TariffViewModel
            {
                UtilityName = t.Utility.UtilityName,
                SlabStart = t.SlabStart,
                SlabEnd = t.SlabEnd,
                RatePerUnit = t.RatePerUnit,
                FixedCharge = t.FixedCharge,
                EffectiveFrom = t.EffectiveFrom.ToDateTime(TimeOnly.MinValue),
                EffectiveTo = t.EffectiveTo!.Value.ToDateTime(TimeOnly.MinValue)
            })
            .ToList();
    }
}
