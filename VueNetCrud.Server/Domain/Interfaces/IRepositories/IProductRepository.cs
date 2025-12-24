using VueNetCrud.Server.Domain.Entities;

namespace VueNetCrud.Server.Domain.Interfaces.Repositories;

public interface IProductRepository
{
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product Add(Product product);
    bool Update(Product product);
    bool Delete(int id);
}

