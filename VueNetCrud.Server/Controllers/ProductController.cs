using Microsoft.AspNetCore.Mvc;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;

namespace VueNetCrud.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all products");
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching product by ID {id}", id);
            var product = await _productService.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductCreateDto dto)
        {
            _logger.LogInformation("Adding a new product: {@product}", dto);
            var created = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            _logger.LogInformation("Updating product Id {id}", id);
            var result = await _productService.UpdateAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogWarning("Deleting product Id {id}", id);
            var result = await _productService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
