using ChatAPI.Application.Contracts;
using ChatAPI.Application.RepositoryDTOs.ChatRepository;
using ChatAPI.Application.RepositoryDTOs.MessageReposiory.SendMessage;
using ChatAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatAPI.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessageReposiory _messageReposiory;

        public MessageController(IMessageReposiory messageReposiory)
        {
            _messageReposiory = messageReposiory;
        }
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageDto.Request request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId)) return BadRequest(ModelState);

            var result = await _messageReposiory.SendMessageAsync(request);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}
