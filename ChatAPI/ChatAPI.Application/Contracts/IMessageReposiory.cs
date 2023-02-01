using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.RepositoryDTOs.MessageReposiory.SendMessage;

namespace ChatAPI.Application.Contracts
{
    public interface IMessageReposiory
    {
        public Task<APIResponse<SendMessageDto.Response>> SendMessageAsync(SendMessageDto.Request request);
        public Task<List<SendMessageDto.Response>> GetAllMessagesByChatIdAsync(Guid chatId);

    }
}
