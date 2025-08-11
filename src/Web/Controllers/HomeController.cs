using System.Diagnostics;
using Application.Features.Categories.Queries.Get;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;
using Web.Models;
using Web.Services.Dispatchers;

namespace Web.Controllers;

public class HomeController(ILogger<HomeController> logger, IRequestDispatcher dispatcher) : BaseController(dispatcher)
{
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var result = await Dispatcher.DispatchAsync(new GetCategoriesQuery());

        return View(result.Data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
