namespace ProductCatalog.Application.Services
{
    public interface IInventoryService
    {
        Task<int> GetStockQuantityAsync(int productId);

        Task UpdateStockQuantityAsync(int productId, int stockQuantity);

        Task DeleteStockQuantityAsync(int productId);
    }
}

