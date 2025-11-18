using VueNetCrud.Server.Models;

namespace VueNetCrud.Server.Services;

public class ItemRepository
{
    private readonly List<Item> _items = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public IReadOnlyList<Item> GetAll()
    {
        lock (_lock)
        {
            return _items.ToList();
        }
    }

    public Item? GetById(int id)
    {
        lock (_lock)
        {
            return _items.FirstOrDefault(i => i.id == id);
        }
    }

    public Item Create(ItemCreate dto)
    {
        var name = dto.name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required", nameof(dto.name));
        }

        lock (_lock)
        {
            var created = new Item(_nextId++, name!, dto.description?.Trim());
            _items.Add(created);
            return created;
        }
    }

    public Item? Update(int id, ItemUpdate dto)
    {
        lock (_lock)
        {
            var existingIndex = _items.FindIndex(i => i.id == id);
            if (existingIndex < 0)
            {
                return null;
            }

            var existing = _items[existingIndex];
            var updated = existing with
            {
                name = string.IsNullOrWhiteSpace(dto.name) ? existing.name : dto.name!.Trim(),
                description = dto.description?.Trim()
            };
            _items[existingIndex] = updated;
            return updated;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            return _items.RemoveAll(i => i.id == id) > 0;
        }
    }
}


