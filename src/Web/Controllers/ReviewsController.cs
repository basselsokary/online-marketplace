using Application.Features.Products.Queries.GetById;
using Application.Features.Reviews.Commands.Create;
using Application.Features.Reviews.Commands.Delete;
using Application.Features.Reviews.Commands.Update;
using Application.Features.Reviews.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;
using Web.Models.Reviews;
using Web.Services.Dispatchers;

namespace Web.Controllers;

[Authorize]
public class ReviewsController(IRequestDispatcher dispatcher) : BaseController(dispatcher)
{
    [HttpGet]
    public async Task<IActionResult> Create(Guid productId)
    {
        var query = new GetProductByIdQuery(productId);
        var result = await Dispatcher.DispatchAsync(query);
        if (!result.Succeeded)
            return View(nameof(NotFound));

        ViewData["ProductName"] = result.Data!.Name;

        var viewModel = new CreateReviewViewModel() { ProductId = productId };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReviewViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var query = new GetProductByIdQuery(viewModel.ProductId);
            var productResult = await Dispatcher.DispatchAsync(query);
            if (!productResult.Succeeded)
                return View(nameof(NotFound));

            ViewData["product"] = productResult.Data;
            return View(viewModel);
        }

        var command = new CreateReviewCommand(viewModel.ProductId, viewModel.Rating, viewModel.Comment);
        var result = await Dispatcher.DispatchAsync(command);
        if (result.Succeeded)
        {
            return RedirectToAction("Details", "Products", new { id = viewModel.ProductId });
        }

        AddModelErrors(result);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await Dispatcher.DispatchAsync(new GetReviewByIdQuery(id));
        if (result == null)
        {
            return View(nameof(NotFound));
        }

        var review = result.Data!;

        var viewModel = new EditReviewViewModel
        {
            Id = review.Id,
            ProductId = review.ProductId,
            Rating = review.Rating,
            Comment = review.Comment
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditReviewViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var command = new UpdateReviewCommand(viewModel.Id, viewModel.Comment, viewModel.Rating);
        var result = await Dispatcher.DispatchAsync(command);
        if (!result.Succeeded)
        {
            return RedirectToAction("Details", "Products", new { id = viewModel.ProductId });
        }

        AddModelErrors(result);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Dispatcher.DispatchAsync(new GetReviewByIdQuery(id));
        if (result == null)
        {
            return View(nameof(NotFound));
        }

        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var result = await Dispatcher.DispatchAsync(new DeleteReviewCommand(id));
        if (!result.Succeeded)
            return View(nameof(NotFound));

        return RedirectToAction(nameof(Index));
    }

}
