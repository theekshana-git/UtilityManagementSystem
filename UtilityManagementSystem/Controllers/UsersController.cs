using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly IUsersService _service;

    public UsersController(IUsersService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var users = _service.GetAll();
        return View(users);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(UsersViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Create", model);

        _service.Create(model);
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Edit(int id)
    {
        var emp = _service.GetById(id);

        var model = new UsersEditViewModel
        {
            EmployeeId = emp.EmployeeId,
            Role = emp.Role,
            Status = emp.Status
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(UsersEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _service.UpdateRoleStatus(model);
        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}