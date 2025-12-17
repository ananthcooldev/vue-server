using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;

namespace VueNetCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ClientCors")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpOptions]
        [HttpOptions("login")]
        public IActionResult Options()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            
            if (result == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(result);
        }
    }
}
