using System.Threading.Tasks;
using Application.Features.Orders.Commands.UpdateStatus;
using Application.Features.Orders.Queries.Get.GetForAdmin;
using Application.Features.Orders.Queries.GetById.ForAdmin;
using Application.Helpers;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;
using Web.Services.Dispatchers;

namespace Web.Controllers;

[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminController(IRequestDispatcher dispatcher) : BaseController(dispatcher)
{
    #region Orders
    [HttpGet("admin/orders")]
    public async Task<IActionResult> Orders()
    {
        var result = await Dispatcher.DispatchAsync(new GetOrdersForAdminQuery());
        return View(result.Data);
    }

    [HttpGet("admin/orders/details")]
    public async Task<IActionResult> OrderDetails(Guid orderId)
    {
        var result = await Dispatcher.DispatchAsync(new GetOrderByIdForAdminQuery(orderId));
        if (result.Succeeded)
            return View(result.Data);

        return View(nameof(NotFound));
    }

    [HttpPost("admin/orders")]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, OrderStatus status)
    {
        var result = await Dispatcher.DispatchAsync(new UpdateOrderStatusCommand(orderId, status));
        if (!result.Succeeded)
        {
            TempData["Error"] = $"Failed to update order {orderId} status.";
            return RedirectToAction(nameof(OrderDetails), new { orderId });
        }

        TempData["Success"] = $"Order {orderId} status updated to {status.GetName()}.";
        return RedirectToAction(nameof(Orders));
    }
    #endregion

}
