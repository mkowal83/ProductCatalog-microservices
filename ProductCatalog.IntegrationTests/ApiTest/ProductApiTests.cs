using FluentAssertions;
using ProductCatalog.Application.DTOs;
using ProductCatalog.IntegrationTests.Common;
using System.Net;
using System.Net.Http.Json;

namespace ProductCatalog.IntegrationTests.ApiTest
{
    public class ProductApiTests : IClassFixture<WebAppFactory>
    {
        private readonly WebAppFactory _webAppFactory;
        private readonly HttpClient _httpClient;

        public ProductApiTests(WebAppFactory webAppFactory)
        {
            _webAppFactory = webAppFactory;
            _httpClient = _webAppFactory.CreateClient();
        }

        [Fact]
        public async Task GetProductById_Should_ReturnStatusCodeOKAndProductDto()
        {
            // Arrange
            int id = 1;

            // Act
            var response = await _httpClient.GetAsync($"/products/{id}");
            var content = await response.Content.ReadFromJsonAsync<ProductDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllProducts_Should_ReturnStatusCodeOKAndListProductDto()
        {
            // Arrange

            // Act
            var response = await _httpClient.GetAsync($"/products");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateProduct_Should_ReturnStatusCodeCreated()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Słuchawki",
                Price = 99.99m,
                StockQuantity = 50
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/product", productDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateProduct_Should_ReturnsNoContent()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Smart TV",
                Price = 1100.0m,
                StockQuantity = 3
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync($"/product/{productDto.Id}", productDto);

            var responseGet = await _httpClient.GetAsync($"/products/{productDto.Id}");
            var retrievedProduct = await responseGet.Content.ReadFromJsonAsync<ProductDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            retrievedProduct.Name.Should().Be(productDto.Name);retrievedProduct.Name.Should().Be(productDto.Name);
            retrievedProduct.Price.Should().Be(productDto.Price);
        }

        [Fact]
        public async Task DeleteProduct_Should_ReturnsNoContent()
        {
            // Arrange
            int id = 2;

            // Act
            var response = await _httpClient.DeleteAsync($"/product/{id}");

            var responseGet = await _httpClient.GetAsync($"/products/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            responseGet.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
