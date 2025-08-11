using Application.Features.Categories.Queries.Get;
using Application.Features.Products.Commands.Create;
using Application.Features.Products.Commands.Delete;
using Application.Features.Products.Commands.Update;
using Application.Features.Products.Queries.Get;
using Application.Features.Products.Queries.GetById;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Controllers.Base;
using Web.Models.Products;
using Web.Services.Dispatchers;

namespace Web.Controllers;

[Authorize]
public class ProductsController(IRequestDispatcher dispatcher) : BaseController(dispatcher)
{
    [AllowAnonymous]
    public async Task<IActionResult> Index(string search, Guid category)
    {
        var categoriesResult = await Dispatcher.DispatchAsync(new GetCategoriesQuery());
        ViewBag.Categories = new SelectList(categoriesResult.Data, "Id", "Name", category);

        var productsResult = await Dispatcher.DispatchAsync(new GetProductsQuery(search, category));

        ViewData["Search"] = search;

        return View(productsResult.Data);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await Dispatcher.DispatchAsync(new GetProductByIdQuery(id));

        if (!result.Succeeded)
            return View("NotFound");

        return View(result.Data);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult> Create()
    {
        await ViewCategories();
        return View();
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateProductCommand(
                viewModel.Name,
                viewModel.Description,
                new(viewModel.PriceAmount, viewModel.PriceCurrency),
                viewModel.StockQuantity,
                viewModel.SelectedCategoryIds,
                viewModel.ImageUrl
            );

            var productResult = await Dispatcher.DispatchAsync(command);
            if (productResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Product created successfully.";
                return RedirectToAction(nameof(Index));
            }

            AddModelErrors(productResult);
        }

        await ViewCategories();
        return View(viewModel);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await Dispatcher.DispatchAsync(new GetProductByIdQuery(id));
        if (!result.Succeeded)
            return View("NotFound");

        var product = result.Data;

        var viewModel = new EditProductViewModel
        {
            Id = product!.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price.Amount,
            StockQuantity = product.UnitsInStock,
            SelectedCategoryIds = product.Categories.Select(c => c.Id).ToList(),
            ImageUrl = product.ImageUrl
        };

        await ViewCategories(viewModel.SelectedCategoryIds);
        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditProductViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await Dispatcher.DispatchAsync(new UpdateProductCommand(
                viewModel.Id,
                viewModel.Name,
                viewModel.Description,
                new(viewModel.Price),
                viewModel.StockQuantity,
                viewModel.SelectedCategoryIds,
                viewModel.ImageUrl
            ));
            
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Product updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            AddModelErrors(result);
        }

        await ViewCategories(viewModel.SelectedCategoryIds);
        return View(viewModel);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Dispatcher.DispatchAsync(new GetProductByIdQuery(id));
        if (!result.Succeeded)
            return View("NotFound");

        return View(result.Data);
    }

    [HttpPost, ActionName(nameof(Delete))]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var result = await Dispatcher.DispatchAsync(new DeleteProductCommand(id));
        if (!result.Succeeded)
            return View("NotFound");

        return RedirectToAction(nameof(Index));
    }

    private async Task ViewCategories(List<Guid>? selectedCategoryIds = null)
    {
        var categories = (await Dispatcher.DispatchAsync(new GetCategoriesQuery())).Data;

        if (selectedCategoryIds == null)
            ViewData["CategoryList"] = new MultiSelectList(categories, "Id", "Name");
        else
            ViewData["CategoryList"] = new MultiSelectList(categories, "Id", "Name", selectedCategoryIds);
    }
}
