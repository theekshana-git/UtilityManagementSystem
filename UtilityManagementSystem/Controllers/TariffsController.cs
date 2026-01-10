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

    // 🔹 Index
    public IActionResult Index()
    {
        ViewBag.Utilities = _service.GetUtilityTypes();
        return View(_service.GetCurrentTariffs());
    }

    // 🔹 Create (GET)
    public IActionResult Create()
    {
        LoadUtilities();

        return View(new TariffViewModel
        {
            EffectiveFrom = DateTime.Today
        });
    }

    // 🔹 Create (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TariffViewModel model)
    {
        if (!ModelState.IsValid)
        {
            LoadUtilities();               // 🔑 extra step
            return View("Create", model);  // 🔑 same pattern as Customers
        }

        _service.AddTariff(model);
        return RedirectToAction(nameof(Index));
    }


    // 🔹 Helper (IMPORTANT)
    private void LoadUtilities()
    {
        ViewBag.Utilities = new SelectList(
            _service.GetUtilityTypes(),
            "UtilityId",
            "UtilityName"
        );
    }

    //history
    public IActionResult History(int utilityId)
    {
        return View(_service.GetTariffHistory(utilityId));
    }

}
