using Microsoft.AspNetCore.Mvc;

namespace UtilityManagementSystem.Controllers
{
    public class PaymentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
