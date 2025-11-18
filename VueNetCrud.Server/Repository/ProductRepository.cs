using VueNetCrud.Server.Models;

namespace VueNetCrud.Server.Repository
{
    public class ProductRepository : IProductRepository
    {
        private static readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 75000, Category = "Electronics" },
            new Product { Id = 2, Name = "Mouse", Price = 500, Category = "Electronics" }
        };

        public IEnumerable<Product> GetAll() => _products;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product Add(Product product)
        {
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
            return product;
        }

        public bool Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing == null) return false;

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Category = product.Category;

            return true;
        }

        public bool Delete(int id)
        {
            var product = GetById(id);
            return product != null && _products.Remove(product);
        }
    }
}
