using Microsoft.EntityFrameworkCore;
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

    // 🔹 Utilities
    public IEnumerable<UtilityType> GetUtilityTypes()
    {
        return _context.UtilityTypes.ToList();
    }

    // 🔹 Add Tariff (versioned by slab)
    public void AddTariff(TariffViewModel model)
    {
        var effectiveFrom = DateOnly.FromDateTime(model.EffectiveFrom);

        // 1️⃣ Expire existing active tariff for SAME slab
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
                ChangedBy = 1, // TODO: logged-in user
                ChangedOn = DateOnly.FromDateTime(DateTime.Now)
            });
        }

        // 2️⃣ Insert new tariff
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

    // 🔹 INDEX: Latest tariff per slab
    public IEnumerable<TariffViewModel> GetLatestTariffsPerSlab()
    {
        return _context.Tariffs
            .Include(t => t.Utility)
            .AsEnumerable() // ⬅️ FORCE SQL → MEMORY
            .GroupBy(t => new
            {
                t.UtilityId,
                t.SlabStart,
                t.SlabEnd
            })
            .Select(g => g
                .OrderByDescending(t => t.EffectiveFrom)
                .First())
            .Select(t => new TariffViewModel
            {
                TariffId = t.TariffId,
                UtilityId = t.UtilityId,
                UtilityName = t.Utility.UtilityName,
                SlabStart = t.SlabStart,
                SlabEnd = t.SlabEnd,
                RatePerUnit = t.RatePerUnit,
                FixedCharge = t.FixedCharge,
                EffectiveFrom = t.EffectiveFrom.ToDateTime(TimeOnly.MinValue),
                EffectiveTo = t.EffectiveTo?.ToDateTime(TimeOnly.MinValue)
            })
            .OrderBy(t => t.UtilityName)
            .ThenBy(t => t.SlabStart)
            .ToList();
    }


    // 🔹 HISTORY: Slab-specific history
    public IEnumerable<TariffViewModel> GetTariffHistoryBySlab(
    int utilityId,
    decimal slabStart,
    decimal? slabEnd)
    {
        return _context.Tariffs
            .Include(t => t.Utility)
            .Where(t =>
                t.UtilityId == utilityId &&
                t.SlabStart == slabStart &&
                t.SlabEnd == slabEnd)
            .OrderByDescending(t => t.EffectiveFrom)
            .Select(t => new TariffViewModel
            {
                UtilityName = t.Utility.UtilityName,
                SlabStart = t.SlabStart,
                SlabEnd = t.SlabEnd,
                RatePerUnit = t.RatePerUnit,
                FixedCharge = t.FixedCharge,
                EffectiveFrom = t.EffectiveFrom.ToDateTime(TimeOnly.MinValue),
                EffectiveTo = t.EffectiveTo != null
                    ? t.EffectiveTo.Value.ToDateTime(TimeOnly.MinValue)
                    : null
            })
            .ToList();
    }

}
