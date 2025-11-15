using FluentAssertions;
using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repositories;

namespace ProductCatalog.UnitTests.ProductServiceTest
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;

        private readonly Mock<IInventoryService> _inventoryServiceMock;

        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _inventoryServiceMock = new Mock<IInventoryService>();

            _productService = new ProductService(_productRepositoryMock.Object, _inventoryServiceMock.Object);
        }

        [Fact]
        public async Task GetProductByIdAsync_WhenProductExists_ShouldReturnProductDto()
        {
            // Arrange
            var productValues = new { Id = 1, Name = "Klawiatura", Price = 9.99m, StockQuantity = 13 };

            var product = new Product
            {
                Id = productValues.Id,
                Name = productValues.Name,
                Price = productValues.Price,
                StockQuantity = productValues.Id,
            };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productValues.Id)).ReturnsAsync(product);
            _inventoryServiceMock.Setup(x => x.GetStockQuantityAsync(productValues.Id)).ReturnsAsync(productValues.StockQuantity);

            // Act
            var result = await _productService.GetProductByIdAsync(productValues.Id);

            // Assert 
            result.Should().NotBeNull();
            result.Id.Should().Be(productValues.Id);
            result.Name.Should().Be(productValues.Name);
            result.Price.Should().Be(productValues.Price);
            result.StockQuantity.Should().Be(productValues.StockQuantity);

            _productRepositoryMock.Verify(x => x.GetByIdAsync(productValues.Id), Times.Once);
            _inventoryServiceMock.Verify(x => x.GetStockQuantityAsync(productValues.Id), Times.Once);
        }

        [Fact]
        public async Task GetAllProductsAsync_WhenProductsExists_ShouldReturnListProductDto()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1056.12m, StockQuantity = 5 },
                new Product { Id = 2, Name = "Myszka komputerowa", Price = 85.00m, StockQuantity = 19 }
            };

            _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(products.Count);
            result.Should().Contain(p => p.Name == "Laptop");
            result.Should().Contain(p => p.Name == "Myszka komputerowa");

            _productRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_WithProduct_ShouldCreateAndReturnProduct()
        {
            // Arrange
            var productDtoValues = new { Name = "Podkładka pod myszkę", Price = 49.99m, StockQuantity = 100 };

            var productDto = new ProductDto
            {
                Name = productDtoValues.Name,
                Price = productDtoValues.Price,
                StockQuantity = productDtoValues.StockQuantity
            };

            _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask).Callback<Product>(x => x.Id = 1);

            // Act
            var result = await _productService.CreateProductAsync(productDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be(productDtoValues.Name);
            result.Price.Should().Be(productDtoValues.Price);
            result.StockQuantity.Should().Be(productDtoValues.StockQuantity);
        }

        [Fact]
        public async Task UpdateProductAsync_WithProduct_ShouldUpdateProduct()
        {
            // Arrange
            var productDtoValues = new { Id = 16, Name = "Głośniki", Price = 129.99m, StockQuantity = 76 };

            var productDto = new ProductDto
            {
                Id = productDtoValues.Id,
                Name = productDtoValues.Name,
                Price = productDtoValues.Price,
                StockQuantity = productDtoValues.StockQuantity
            };

            _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _inventoryServiceMock.Setup(x => x.UpdateStockQuantityAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateProductAsync(productDto);

            // Assert
            _productRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Product>( p => p.Id == productDtoValues.Id 
                                                                               && p.Name == productDtoValues.Name 
                                                                               && p.Price == productDtoValues.Price)), Times.Once);

            _inventoryServiceMock.Verify(x => x.UpdateStockQuantityAsync(productDtoValues.Id, productDtoValues.StockQuantity), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_WhenProductExists_ShouldDeleteProductAndInventory()
        {
            // Arrange
            var productDtoValues = new { Id = 132, Name = "Słuchawki", Price = 459.01m, StockQuantity = 32 };

            var product = new Product
            {
                Id = productDtoValues.Id,
                Name = productDtoValues.Name,
                Price = productDtoValues.Price,
                StockQuantity = productDtoValues.StockQuantity
            };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productDtoValues.Id)).ReturnsAsync(product);

            _productRepositoryMock.Setup(x => x.DeleteAsync(productDtoValues.Id)).Returns(Task.CompletedTask);

            _inventoryServiceMock.Setup(x => x.DeleteStockQuantityAsync(productDtoValues.Id)).Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProductAsync(productDtoValues.Id);

            // Assert
            _productRepositoryMock.Verify(x => x.GetByIdAsync(productDtoValues.Id), Times.Once);
            _productRepositoryMock.Verify(x => x.DeleteAsync(productDtoValues.Id), Times.Once);
            _inventoryServiceMock.Verify(x => x.DeleteStockQuantityAsync(productDtoValues.Id), Times.Once);
        }
    }
}
