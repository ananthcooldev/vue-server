using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;

namespace VueNetCrud.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(IItemService itemService, ILogger<ItemsController> logger)
    {
        _itemService = itemService;
        _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetAll()
    {
        var items = await _itemService.GetAllAsync();
        _logger.LogInformation("GetAll Item API was called at {time}", DateTime.UtcNow);
        return Ok(items);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemResponseDto>> GetById(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        _logger.LogInformation("GetItem by ID {Id} API was called at {time}", id, DateTime.UtcNow);

        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ItemResponseDto>> Create(ItemCreateDto dto)
    {
        try
        {
            var created = await _itemService.CreateAsync(dto);
            _logger.LogInformation("Create Item API was called at {time}", DateTime.UtcNow);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ItemResponseDto>> Update(int id, ItemUpdateDto dto)
    {
        var updated = await _itemService.UpdateAsync(id, dto);
        _logger.LogInformation("Update Item by ID {Id} API was called at {time}", id, DateTime.UtcNow);

        if (updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removed = await _itemService.DeleteAsync(id);
        _logger.LogInformation("Delete Item by ID {Id} API was called at {time}", id, DateTime.UtcNow);

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
        _logger.LogInformation("TestError API was called at {time}", DateTime.UtcNow);
        throw new Exception("Test exception for logging");
    }
}
