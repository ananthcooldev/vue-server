using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Services;

namespace VueNetCrud.Server.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ItemsController : ControllerBase
{
    private readonly ItemRepository _repository;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(ItemRepository repository, ILogger<ItemsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetAll()
    {
        var items = _repository.GetAll();
        _logger.LogInformation("GetAll Item API was at {time}", DateTime.UtcNow);
        return Ok(items);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public ActionResult<Item> GetById(int id)
    {
        var item = _repository.GetById(id);
        _logger.LogInformation("GetAll Item by ID API was at {time}", DateTime.UtcNow);

        if (item is null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [Authorize]
    [HttpPost]
    public ActionResult<Item> Create(ItemCreate dto)
    {
        try
        {
            var created = _repository.Create(dto);
            _logger.LogInformation("Create Item by model Object API was at {time}", DateTime.UtcNow);

            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public ActionResult<Item> Update(int id, ItemUpdate dto)
    {
        var updated = _repository.Update(id, dto);
        _logger.LogInformation("Update Item by model Object API was at {time}", DateTime.UtcNow);

        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var removed = _repository.Delete(id);
        _logger.LogInformation("Delete Item by Id API was at {time}", DateTime.UtcNow);

        if (!removed)
        {
            return NotFound();
        }
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("TestError")]
    public IActionResult TestError()
    {
        _logger.LogInformation("TestError API was at {time}", DateTime.UtcNow);
        throw new Exception("Test exception for logging");
    }
}



