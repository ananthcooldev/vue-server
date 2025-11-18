using VueNetCrud.Server.Validators;

namespace VueNetCrud.Server.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        [ValidCategory]
        public string Category { get; set; } = string.Empty;
    }
}
