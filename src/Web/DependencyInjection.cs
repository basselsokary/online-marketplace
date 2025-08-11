using Microsoft.AspNetCore.Mvc.Routing;
using Web.Services.Dispatchers;

namespace Web;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersWithViews();

        // Cookies, Session, Cache
        services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache

        services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Auth/AccessDenied";
            options.LoginPath = "/Auth/Login";

        });
        
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true; // For GDPR compliance
        });

        return services.AddServices();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IRequestDispatcher, RequestDispatcher>();

        return services;
    }

}
