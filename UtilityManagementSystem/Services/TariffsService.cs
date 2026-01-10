using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

public class TariffService : ITariffService
{
    private readonly UtilityDbContext _context;

    public TariffService(UtilityDbContext context)
    {
        _context = context;
    }

    public IEnumerable<UtilityType> GetUtilityTypes()
    {
        return _context.UtilityTypes.ToList();
    }


    // =========================
    // ADD TARIFF (BUSINESS RULE)
    // =========================
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

    // =========================
    // CURRENT TARIFFS
    // =========================
    public IEnumerable<TariffViewModel> GetCurrentTariffs()
    {
        return _context.Tariffs
            .Where(t => t.EffectiveTo == null)
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

    // =========================
    // TARIFF HISTORY
    // =========================
    public IEnumerable<TariffViewModel> GetTariffHistory(int utilityId)
    {
        return _context.Tariffs
            .Where(t => t.UtilityId == utilityId && t.EffectiveTo != null)
            .OrderByDescending(t => t.EffectiveFrom)
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
