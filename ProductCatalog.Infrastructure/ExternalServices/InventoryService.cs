using ProductCatalog.Application.Services;

namespace ProductCatalog.Infrastructure.ExternalServices
{
    public class InventoryService : IInventoryService
    {

        public async Task<int> GetStockQuantityAsync(int productId)
        {
            // Symulacja komunikacji z innym mikroserwisem
            await Task.Delay(100); // Symulacja opóźnienia sieciowego
            return new Random().Next(0, 100);
        }

        public async Task UpdateStockQuantityAsync(int productId, int stockQuantity)
        {
            // Symulacja komunikacji z innym mikroserwisem
            await Task.Delay(100); // Symulacja opóźnienia sieciowego
        }

        public async Task DeleteStockQuantityAsync(int productId)
        {
            // Symulacja komunikacji z innym mikroserwisem
            await Task.Delay(100); // Symulacja opóźnienia sieciowego
        }
    }
}
