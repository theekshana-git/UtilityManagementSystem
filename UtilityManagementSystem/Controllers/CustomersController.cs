using System;
using Microsoft.AspNetCore.Mvc;
using UtilityManagementSystem.Services.Interfaces;
using UtilityManagementSystem.ViewModels;

public class CustomersController : Controller
{
    private readonly ICustomersService _service;

    public CustomersController(ICustomersService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View(_service.GetAllCustomers());
    }

    // ---------- Create ----------
    public IActionResult Create()
    {
        var model = new CustomersViewModel
        {
            Register = DateTime.Today  //set default date as today
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CustomersViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Create", model);
        }


        _service.AddCustomer(model);
        return RedirectToAction(nameof(Index));
    }

    // ---------- Edit ----------
    public IActionResult Edit(int id)
    {
        var customer = _service.GetCustomerById(id);
        if (customer == null)
            return NotFound();

        var model = new CustomerEditViewModel
        {
            CustomerID = customer.CustomerID,
            Name = customer.Name,
            Type = customer.Type,
            City = customer.City,
            Contact = customer.Contact
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public IActionResult Edit(CustomerEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _service.UpdateCustomer(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult ToggleActive(int id)
    {
        _service.ToggleActive(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int id)
    {
        var model = _service.GetCustomerDetails(id);

        if (model == null)
            return NotFound();

        return View(model);
    }

}