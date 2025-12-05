using Microsoft.AspNetCore.Mvc;

namespace UtilityManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
