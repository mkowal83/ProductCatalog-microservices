using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Mappings;
using ProductCatalog.Domain.Repositories;

namespace ProductCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly IInventoryService _inventoryService;

        public ProductService(IProductRepository productRepository, IInventoryService inventoryService)
        {
            _productRepository = productRepository;
            _inventoryService = inventoryService;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if(product == null)
            {
                return null;
            }

            // pobranie z innego serwisu informacji o stanie magazynowym
            var stockQuantity = await _inventoryService.GetStockQuantityAsync(id);
            product.StockQuantity = stockQuantity;

            return ProductMapper.ToDto(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(ProductMapper.ToDto).ToList();
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var product = ProductMapper.ToEntity(productDto);

            await _productRepository.AddAsync(product);

            return ProductMapper.ToDto(product);
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var product = ProductMapper.ToEntity(productDto);   

            await _productRepository.UpdateAsync(product);

            // aktualizacja w innym serwisie informacji o stanie magazynowym
            await _inventoryService.UpdateStockQuantityAsync(product.Id, product.StockQuantity);
        }

        public async Task DeleteProductAsync(int id)
        {
            // sprawdzenie czy produkt istnieje
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return;
            }

            await _productRepository.DeleteAsync(id);

            // usunięcie w innym serwisie informacji o stanie magazynowym, bo produkt został usunięty
            await _inventoryService.DeleteStockQuantityAsync(id);
        }

    }
}
