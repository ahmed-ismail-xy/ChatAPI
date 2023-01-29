using ChatAPI.Application.Contracts;
using ChatAPI.Application.RepositoryDTOs.AuthRepository;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
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
            return result.Success ? (IActionResult)Ok(result.Data) : BadRequest(result.Message);
        }
    }
}
