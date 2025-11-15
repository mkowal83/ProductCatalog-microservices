using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Services;

namespace ProductCatalog.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
