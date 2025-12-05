using Microsoft.AspNetCore.Mvc;

namespace UtilityManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
