using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;

namespace ProductCatalog.UnitTests.ProductRepositoryTest
{
    public class ProductRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> options;

        public ProductRepositoryTests()
        {
            options = GetDbContextOptions();
        }

        private DbContextOptions<ApplicationDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                       .UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid().ToString()).Options;
        }

        private async Task FillInMemoryDataBasic(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                await context.Products.AddRangeAsync
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

                await context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                await FillInMemoryDataBasic(context);
            }

            Product product;

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var productRepository = new ProductRepository(context);

                product = await productRepository.GetByIdAsync(1);
            }

            // Assert
            product.Should().NotBeNull();
            product.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                await FillInMemoryDataBasic(context);
            }

            int count = 0;

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var productRepository = new ProductRepository(context);

                var products = await productRepository.GetAllAsync();

                count = products.Count();
            }

            // Assert
            count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduct()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                await FillInMemoryDataBasic(context);
            }

            var product = new Product
            {
                Name = "Smartphone",
                Price = 799.99m,
                StockQuantity = 25
            };

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var productRepository = new ProductRepository(context);

                await productRepository.AddAsync(product);
            }

            // Assert
            product.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyProduct()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                await FillInMemoryDataBasic(context);
            }

            Product product;
            var updatedName = "Smart TV";
            var updatedPrice = 899.99m;

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var productRepository = new ProductRepository(context);

                product = await productRepository.GetByIdAsync(1);

                product.Name = updatedName;
                product.Price = updatedPrice;

                await productRepository.UpdateAsync(product);
            }

            // Assert
            product.Should().NotBeNull();
            product.Name.Should().Be(updatedName);
            product.Price.Should().Be(updatedPrice);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                await FillInMemoryDataBasic(context);
            }

            int id = 1;
            Product product;
            Product productRemove;

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var productRepository = new ProductRepository(context);

                product = await productRepository.GetByIdAsync(id);

                await productRepository.DeleteAsync(product.Id);

                productRemove = await productRepository.GetByIdAsync(id);
            }

            // Assert
            product.Should().NotBeNull();
            productRemove.Should().BeNull();
        }
    }
}
