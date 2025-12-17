using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Domain.Interfaces.Repositories;

namespace VueNetCrud.Server.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = _repository.GetAll();
        var dtos = products.Select(p => new ProductResponseDto(p.Id, p.Name, p.Price, p.Category));
        return Task.FromResult(dtos.AsEnumerable());
    }

    public Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = _repository.GetById(id);
        if (product == null)
            return Task.FromResult<ProductResponseDto?>(null);

        return Task.FromResult<ProductResponseDto?>(new ProductResponseDto(product.Id, product.Name, product.Price, product.Category));
    }

    public Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
    {
        var product = new Product
        {
            Id = 0,
            Name = dto.Name,
            Price = dto.Price,
            Category = dto.Category
        };

        var created = _repository.Add(product);
        _logger.LogInformation("Product created with ID {Id}", created.Id);
        
        return Task.FromResult(new ProductResponseDto(created.Id, created.Name, created.Price, created.Category));
    }

    public Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
    {
        if (id != dto.Id)
            return Task.FromResult(false);

        var product = new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Category = dto.Category
        };

        var result = _repository.Update(product);
        if (result)
        {
            _logger.LogInformation("Product updated with ID {Id}", id);
        }
        return Task.FromResult(result);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var result = _repository.Delete(id);
        if (result)
        {
            _logger.LogWarning("Product deleted with ID {Id}", id);
        }
        return Task.FromResult(result);
    }
}

