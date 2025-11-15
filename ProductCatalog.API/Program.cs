using ProductCatalog.Application;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.local.json");
}

// Add services to the container.

builder.Services.AddSqlDatabase(builder.Configuration.GetConnectionString("MainDbSql")!);

builder.Services.AddApplicationServices();

if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

// Minimal API

app.MapGet("/products/{id}", async (int id, IProductService productService) =>
{
    var product = await productService.GetProductByIdAsync(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById");


app.MapGet("/products", async (IProductService productService) =>
{
    return await productService.GetAllProductsAsync();
}).WithName("GetAllProducts");


app.MapPost("/product", async (ProductDto productDto, IProductService productService) =>
{
    var productCreated = await productService.CreateProductAsync(productDto);

    return Results.Created($"/products/{productCreated.Id}", productCreated);
}).WithName("CreateProduct");


app.MapPut("/product/{id}", async (ProductDto productDto, IProductService productService) =>
{
    await productService.UpdateProductAsync(productDto);

    return Results.NoContent();
}).WithName("UpdateProduct");


app.MapDelete("/product/{id}", async (int id, IProductService productService) =>
{
    await productService.DeleteProductAsync(id);

    return Results.NoContent();
}).WithName("DeleteProduct");


app.Run();


public partial class Program { }