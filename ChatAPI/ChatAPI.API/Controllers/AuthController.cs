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
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromForm] RegisterDTO.Request request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.RegisterUserAsync(request);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUserAsync([FromForm] LoginDTO.Request request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.LoginUserAsync(request);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
