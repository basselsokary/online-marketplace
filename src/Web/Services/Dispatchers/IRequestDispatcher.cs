using Application.Common.Interfaces.Messaging.Requests.Base;

namespace Web.Services.Dispatchers;

public interface IRequestDispatcher
{
    Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}