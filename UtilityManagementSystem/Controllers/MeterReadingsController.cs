using Microsoft.AspNetCore.Mvc;

namespace UtilityManagementSystem.Controllers
{
    public class MeterReadingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
