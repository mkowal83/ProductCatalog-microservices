using ProductCatalog.Domain.Entities;
using ProductCatalog.Application.Mappings;
using FluentAssertions;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.UnitTests.ProductMapperTest
{
    public class ProductMapperTests
    {
        public static IEnumerable<object[]> Products = new List<object[]>
        {
          new object[] { new Product { Id = 1, Name = "Laptop", Price = 1056.12m, StockQuantity = 5 } },
          new object[] { new Product { Id = 2, Name = "Myszka komputerowa", Price = 85.00m, StockQuantity = 19 } }
        };

        public static IEnumerable<object[]> ProductsDto = new List<object[]>
        {
          new object[] { new ProductDto { Id = 6, Name = "Komputer stacjonarny", Price = 4999.99m, StockQuantity = 5 } },
          new object[] { new ProductDto { Id = 7, Name = "Monitor", Price = 1500.00m, StockQuantity = 19 } }
        };


        [Theory]
        [MemberData(nameof(Products))]
        public void ToDto_Should_Map_Product_To_ProductDto(Product product)
        {
            // Arrange

            // Act
            var productDto = ProductMapper.ToDto(product);

            // Assert
            productDto.Should().NotBeNull();
            productDto.Id.Should().Be(product.Id);
            productDto.Name.Should().Be(product.Name);
            productDto.Price.Should().Be(product.Price);
            productDto.StockQuantity.Should().Be(product.StockQuantity);
        }

        [Theory]
        [MemberData(nameof(ProductsDto))]
        public void ToEntity_Should_Map_ProductDto_To_Product(ProductDto productDto)
        {
            // Arrange

            // Act
            var product = ProductMapper.ToEntity(productDto);

            // Assert
            product.Should().NotBeNull();
            product.Id.Should().Be(productDto.Id);
            product.Name.Should().Be(productDto.Name);
            product.Price.Should().Be(productDto.Price);
            product.StockQuantity.Should().Be(product.StockQuantity);
        }
    }
}
