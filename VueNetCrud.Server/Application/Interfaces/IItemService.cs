using VueNetCrud.Server.Application.DTOs;

namespace VueNetCrud.Server.Application.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemResponseDto>> GetAllAsync();
    Task<ItemResponseDto?> GetByIdAsync(int id);
    Task<ItemResponseDto> CreateAsync(ItemCreateDto dto);
    Task<ItemResponseDto?> UpdateAsync(int id, ItemUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

