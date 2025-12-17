using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Domain.Interfaces.Repositories;

namespace VueNetCrud.Server.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repository;
    private readonly ILogger<ItemService> _logger;

    public ItemService(IItemRepository repository, ILogger<ItemService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public Task<IEnumerable<ItemResponseDto>> GetAllAsync()
    {
        var items = _repository.GetAll();
        var dtos = items.Select(i => new ItemResponseDto(i.Id, i.Name, i.Description));
        return Task.FromResult(dtos.AsEnumerable());
    }

    public Task<ItemResponseDto?> GetByIdAsync(int id)
    {
        var item = _repository.GetById(id);
        if (item == null)
            return Task.FromResult<ItemResponseDto?>(null);

        return Task.FromResult<ItemResponseDto?>(new ItemResponseDto(item.Id, item.Name, item.Description));
    }

    public Task<ItemResponseDto> CreateAsync(ItemCreateDto dto)
    {
        var name = dto.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required", nameof(dto.Name));
        }

        var item = new Item(0, name!, dto.Description?.Trim());
        var created = _repository.Create(item);
        _logger.LogInformation("Item created with ID {Id}", created.Id);
        
        return Task.FromResult(new ItemResponseDto(created.Id, created.Name, created.Description));
    }

    public Task<ItemResponseDto?> UpdateAsync(int id, ItemUpdateDto dto)
    {
        var existing = _repository.GetById(id);
        if (existing == null)
            return Task.FromResult<ItemResponseDto?>(null);

        var updatedItem = existing with
        {
            Name = string.IsNullOrWhiteSpace(dto.Name) ? existing.Name : dto.Name!.Trim(),
            Description = dto.Description?.Trim()
        };

        var updated = _repository.Update(id, updatedItem);
        if (updated == null)
            return Task.FromResult<ItemResponseDto?>(null);

        _logger.LogInformation("Item updated with ID {Id}", id);
        return Task.FromResult<ItemResponseDto?>(new ItemResponseDto(updated.Id, updated.Name, updated.Description));
    }

    public Task<bool> DeleteAsync(int id)
    {
        var result = _repository.Delete(id);
        if (result)
        {
            _logger.LogInformation("Item deleted with ID {Id}", id);
        }
        return Task.FromResult(result);
    }
}

