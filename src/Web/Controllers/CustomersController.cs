using Application.DTOs;
using Application.Features.Customers.Commands.Update;
using Application.Features.Customers.Queries.GetAll;
using Application.Features.Customers.Queries.Profile;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;
using Web.Models.Customers;
using Web.Services.Dispatchers;

namespace Web.Controllers;

[Authorize]
public class CustomersController(IRequestDispatcher dispatcher) : BaseController(dispatcher)
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> CustomersForAdmin()
    {
        var result = await Dispatcher.DispatchAsync(new GetAllCustomersQuery());
        if (!result.Succeeded)
            return View("Error");

        return View(result.Data);
    }

    [Authorize(Roles = nameof(UserRole.Customer))]
    public async Task<IActionResult> Profile()
    {
        var result = await Dispatcher.DispatchAsync(new GetProfileQuery());
        if (!result.Succeeded)
            return View("NotFound");

        var customerDto = result.Data!;

        var customerVM = new CustomerUserViewModel
        {
            UserName = customerDto.UserName,
            Email = customerDto.Email,
            FullName = customerDto.FullName,
            Street = customerDto.Address.Street,
            City = customerDto.Address.City,
            District = customerDto.Address.District,
            ZipCode = customerDto.Address.ZipCode,
            Age = customerDto.Age,
            PhoneNumber = customerDto.PhoneNumber,
            DateJoined = customerDto.DateJoined
        };

        return View(customerVM);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Customer))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(CustomerUserViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var addressDto = new AddressDto(viewModel.Street, viewModel.District, viewModel.City, viewModel.ZipCode);
        var command = new UpdateCustomerCommand(viewModel.FullName, addressDto, viewModel.PhoneNumber, viewModel.Age);
        var result = await Dispatcher.DispatchAsync(command);
        if (!result.Succeeded)
        {
            AddModelErrors(result);
        }

        return RedirectToAction(nameof(Profile));
    }

}
