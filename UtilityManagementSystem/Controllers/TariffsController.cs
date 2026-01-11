using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

public class TariffsController : Controller
{
    private readonly ITariffsService _service;

    public TariffsController(ITariffsService service)
    {
        _service = service;
    }

    // 🔹 INDEX — only latest tariff per slab
    public IActionResult Index()
    {
        ViewBag.Utilities = _service.GetUtilityTypes();

        // MUST return only latest slab-wise tariffs
        var currentTariffs = _service.GetLatestTariffsPerSlab();

        return View(_service.GetLatestTariffsPerSlab());
    }

    // 🔹 CREATE (GET)
    public IActionResult Create()
    {
        LoadUtilities();

        return View(new TariffViewModel
        {
            EffectiveFrom = DateTime.Today
        });
    }

    // 🔹 CREATE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TariffViewModel model)
    {
        if (!ModelState.IsValid)
        {
            LoadUtilities();
            return View("Create", model);
        }

        _service.AddTariff(model);
        return RedirectToAction(nameof(Index));
    }

    // 🔹 HISTORY — specific slab history
    public IActionResult History(int utilityId, decimal slabStart, decimal? slabEnd)
    {
        var history = _service.GetTariffHistoryBySlab(
            utilityId,
            slabStart,
            slabEnd
        );

        return View(history);
    }


    // 🔹 Helper
    private void LoadUtilities()
    {
        ViewBag.Utilities = new SelectList(
            _service.GetUtilityTypes(),
            "UtilityId",
            "UtilityName"
        );
    }
}
