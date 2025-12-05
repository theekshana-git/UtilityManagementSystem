using Microsoft.AspNetCore.Mvc;

namespace UtilityManagementSystem.Controllers
{
    public class ComplaintsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
