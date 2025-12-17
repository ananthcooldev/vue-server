using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Domain.Interfaces.Repositories;

namespace VueNetCrud.Server.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
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
            return _items.FirstOrDefault(i => i.Id == id);
        }
    }

    public Item Create(Item item)
    {
        lock (_lock)
        {
            var created = new Item(_nextId++, item.Name, item.Description);
            _items.Add(created);
            return created;
        }
    }

    public Item? Update(int id, Item item)
    {
        lock (_lock)
        {
            var existingIndex = _items.FindIndex(i => i.Id == id);
            if (existingIndex < 0)
            {
                return null;
            }

            var updated = item with { Id = id };
            _items[existingIndex] = updated;
            return updated;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            return _items.RemoveAll(i => i.Id == id) > 0;
        }
    }
}

