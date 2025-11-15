using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.Data;

namespace ProductCatalog.IntegrationTests.Common
{
    public class WebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureTestServices(services =>
            {
                // Usunięcie poprzedniej rejestracji DbContext, który używał rzeczywistej bazy danych
                var dbContextOptions = services.Where(d => d.ServiceType.Name.Contains("DbContextOptions")).ToList();

                foreach (var d in dbContextOptions)
                {
                    services.Remove(d);
                }

                // Utworzenie nowego DbContext (InMemory) na potrzeby testów
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatabaseTest");
                });


                // Zainicjaliowanie bazy InMemory
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                // Zasilenie bazy danymi testowymi
               FillInMemoryDataBasic(context);
            });
        }

        private void FillInMemoryDataBasic(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                context.Products.AddRange
                (
                   new Product
                   {
                       Name = "Telewizor",
                       Price = 1000.0m,
                       StockQuantity = 2
                   },
                   new Product
                   {
                       Name = "Radio",
                       Price = 167.12m,
                       StockQuantity = 0
                   },
                   new Product
                   {
                       Name = "Tablet",
                       Price = 1300.0m,
                       StockQuantity = 66
                   }
                );

                context.SaveChanges();
            }
        }
    }
}
