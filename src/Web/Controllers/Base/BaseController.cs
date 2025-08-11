using Microsoft.AspNetCore.Mvc;
using SharedKernel.Models;
using Web.Services.Dispatchers;

namespace Web.Controllers.Base;

public abstract class BaseController : Controller
{
    protected readonly IRequestDispatcher Dispatcher;

    protected BaseController(IRequestDispatcher dispatcher) => Dispatcher = dispatcher;

    protected void AddModelErrors(Result result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error);
        }
    }
}
