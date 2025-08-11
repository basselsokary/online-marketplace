using Application.Features.Orders.Commands.Cancel;
using Application.Features.Orders.Commands.UpdateStatus;
using Application.Features.Orders.Queries.Get.GetForAdmin;
using Application.Features.Orders.Queries.Get.GetForCustomer;
using Application.Features.Orders.Queries.GetById.ForAdmin;
using Application.Features.Orders.Queries.GetById.ForCustomer;
using Application.Helpers;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;
using Web.Models.Orders;
using Web.Services.Dispatchers;

namespace Web.Controllers;

[Authorize(Roles = nameof(UserRole.Customer))]
public class OrdersController(IRequestDispatcher dispatcher) : BaseController(dispatcher)
{    
    public async Task<ActionResult> Index()
    {
        var result = await Dispatcher.DispatchAsync(new GetOrdersForCustomerQuery());
        return View(result.Data);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var query = new GetOrderByIdForCustomerQuery(id);
        var result = await Dispatcher.DispatchAsync(query);

        if (!result.Succeeded)
            return View(nameof(NotFound));

        return View(result.Data);
    }
	
    public async Task<IActionResult> Cancel(Guid id)
    {
        var result = await Dispatcher.DispatchAsync(new GetOrderByIdForCustomerQuery(id));
        if (!result.Succeeded)
            return View(nameof(NotFound));

        return View(result.Data);
    }
	
    [HttpPost, ActionName("Cancel")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelConfirmed(Guid id)
    {
        var result = await Dispatcher.DispatchAsync(new CancelOrderCommand(id));
        if (!result.Succeeded)
        {
            AddModelErrors(result);
			return RedirectToAction(nameof(Details), new { id });
        }
        
        return RedirectToAction(nameof(Index));
    }

}
