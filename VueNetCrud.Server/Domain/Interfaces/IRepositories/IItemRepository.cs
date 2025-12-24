using VueNetCrud.Server.Domain.Entities;

namespace VueNetCrud.Server.Domain.Interfaces.Repositories;

public interface IItemRepository
{
    IReadOnlyList<Item> GetAll();
    Item? GetById(int id);
    Item Create(Item item);
    Item? Update(int id, Item item);
    bool Delete(int id);
}

