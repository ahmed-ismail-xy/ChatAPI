using ChatAPI.Application.Contracts;
using ChatAPI.Application.RepositoryDTOs.AuthRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthRepository authRepository, ILogger<AuthController> logger)
        {
            _authRepository = authRepository;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterDTO.Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authRepository.RegisterUserAsync(request);
            return result.Success ? (IActionResult)Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromForm] LoginDTO.Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authRepository.LoginUserAsync(request);
            
           // _logger.LogDebug($"The response for the Login is { JsonConvert.SerializeObject(result)}");  
            return result.Success ? (IActionResult)Ok(result.Data) : BadRequest(result.Message);
        }
    }
}
