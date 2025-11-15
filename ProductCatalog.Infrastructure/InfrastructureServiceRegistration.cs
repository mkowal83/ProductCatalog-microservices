using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Services;
using ProductCatalog.Domain.Repositories;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.ExternalServices;
using ProductCatalog.Infrastructure.Repositories;

namespace ProductCatalog.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddSqlDatabase(this IServiceCollection services, string connectionString)
        {
            Action<IServiceProvider, DbContextOptionsBuilder> sqlOptions = (serviceProvider, options) => options.UseSqlServer(connectionString);

            services.AddDbContext<ApplicationDbContext>(sqlOptions);

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IInventoryService, InventoryService>();

            return services;
        }
    }
}
