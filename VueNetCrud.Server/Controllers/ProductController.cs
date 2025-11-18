using Microsoft.AspNetCore.Mvc;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Repository;

namespace VueNetCrud.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository repo, ILogger<ProductController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Fetching all products");
            return Ok(_repo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation("Fetching product by ID {id}", id);
            var product = _repo.GetById(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            _logger.LogInformation("Adding a new product: {@product}", product);
            var created = _repo.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            _logger.LogInformation("Updating product Id {id}", id);
            return _repo.Update(product) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogWarning("Deleting product Id {id}", id);
            return _repo.Delete(id) ? NoContent() : NotFound();
        }
    }
}
