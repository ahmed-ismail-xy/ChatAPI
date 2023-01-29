using ChatAPI.Application.Contracts;
using ChatAPI.Application.RepositoryDTOs.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatAPI.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (String.IsNullOrEmpty(userId))
                return BadRequest("User ID not found.");

            var result = await _userRepository.GetUserAsync(Guid.Parse(userId));

            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("GetListUsers")]
        public async Task<IActionResult> GetListAllUsers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (String.IsNullOrEmpty(userId))
                return BadRequest("User ID not found.");

            var result = await _userRepository.GetListAllUsersAsync(Guid.Parse(userId));

            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO.Request request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (String.IsNullOrEmpty(userId))
                return BadRequest("User ID not found.");

            var result = await _userRepository.UpdateUserAsync(Guid.Parse(userId), request);

            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPost("UpdateUserProfileImage")]
        public async Task<IActionResult> UpdateUserProfileImage([FromForm] IFormFile userProfileImage)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (String.IsNullOrEmpty(userId))
                return BadRequest("User ID not found.");

            var result = await _userRepository.UpdateUserProfileImageAsync(Guid.Parse(userId), userProfileImage);

            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete("DeleteProfileImage")]
        public async Task<IActionResult> DeleteUserProfileImage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (String.IsNullOrEmpty(userId))
                return BadRequest("User ID not found.");

            var result = await _userRepository.DeleteUserProfileImageAsync(Guid.Parse(userId));

            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}