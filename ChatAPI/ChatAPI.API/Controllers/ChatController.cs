using ChatAPI.Application.Contracts;
using ChatAPI.Application.RepositoryDTOs.ChatRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatAPI.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        [HttpPost("CreateChat")]
        public async Task<IActionResult> CreateChat([FromForm] Guid receiverId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId)) return BadRequest(ModelState);

            var request = new CreateChatDTO.Request
            {
                ReceiverId = receiverId,
                SenderId = Guid.Parse(userId)
            };
            var result = await _chatRepository.CreateChatAsync(request);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("GetChats")]
        public async Task<IActionResult> GetAllChats()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId)) return BadRequest(ModelState);

            var result = await _chatRepository.GetListAllChatsAsync(Guid.Parse(userId));

            return result.Success ? Ok(result.Data) : BadRequest(result.Message);

        }
    }
}
