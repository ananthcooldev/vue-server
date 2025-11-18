using VueNetCrud.Server.Models;

namespace VueNetCrud.Server.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product? GetById(int id);
        Product Add(Product product);
        bool Update(Product product);
        bool Delete(int id);
    }
}
