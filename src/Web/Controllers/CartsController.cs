using Application.DTOs;
using Application.Features.Carts.Commands.AddItem;
using Application.Features.Carts.Commands.Clear;
using Application.Features.Carts.Commands.RemoveItem;
using Application.Features.Carts.Commands.UpdateItem;
using Application.Features.Carts.Queries.Get;
using Application.Features.Orders.Commands.Place;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web.Controllers.Base;
using Web.Models.Carts;
using Web.Services.Dispatchers;

namespace Web.Controllers;

[Authorize(Roles = nameof(UserRole.Customer))]
public class CartsController(IRequestDispatcher dispatcher) : BaseController(dispatcher)
{
    public async Task<IActionResult> Index()
    {
        var result = await Dispatcher.DispatchAsync(new GetCartItemsQuery());
        ViewData["c_error"] = result.Errors;
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> AddItem(Guid productId, int quantity = 1)
    {
        var command = new AddCartItemCommand(productId, quantity);
        var result = await Dispatcher.DispatchAsync(command);
        if (!result.Succeeded)
        {
            AddModelErrors(result);
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            // return RedirectToAction(nameof(Index));
        }

        return Created();
        // return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItem(Guid itemId)
    {
        var command = new RemoveCartItemCommand(itemId);
        var result = await Dispatcher.DispatchAsync(command);
        if (!result.Succeeded)
        {
            AddModelErrors(result);
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateItemQuantity(Guid itemId, int quantity)
    {
        var command = new UpdateCartItemQuantityCommand(itemId, quantity);
        var result = await Dispatcher.DispatchAsync(command);
        if (!result.Succeeded)
        {
            AddModelErrors(result);
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

        return NoContent();
    }

    public async Task<IActionResult> Checkout()
    {
        var result = await Dispatcher.DispatchAsync(new GetCartItemsQuery());
        if (!result.Succeeded)
        {
            AddModelErrors(result);
            return RedirectToAction(nameof(Index));
        }

        var cartItems = result.Data!.CartItems;

        var viewModel = new CheckoutViewModel
        {
            CartItems = cartItems.Select(ci => new CartItemViewModel()
            {
                Id = ci.Id,
                ProductName = ci.ProductName,
                Price = ci.ProductPrice.Amount,
                Quantity = ci.Quantity
            }).ToList(),
            Total = result.Data.TotalPrice
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel viewModel)
    {
        var command = new PlaceOrderCommand(viewModel.Address, viewModel.PaymentMethod);
        var result = await Dispatcher.DispatchAsync(command);
        if (!result.Succeeded)
        {
            AddModelErrors(result);
            return RedirectToAction(nameof(Index));
        }

        if (viewModel.PaymentMethod != PaymentMethod.Cash)
        {
            // Should process payment. By backgroung job, if payment succeessful, the stock should be decreased.
        }

        // Clear the cart after successful checkout
        await Dispatcher.DispatchAsync(new ClearCartCommand());

        // Redirect to order confirmation
        return RedirectToAction("Details", "Orders", new { id = result.Data });
    }

    [HttpPost]
    public async Task<IActionResult> ClearCart()
    {
        var result = await Dispatcher.DispatchAsync(new ClearCartCommand());
        if (result.Succeeded)
            return RedirectToAction(nameof(Index));

        return BadRequest(result.Errors);
    }

}
