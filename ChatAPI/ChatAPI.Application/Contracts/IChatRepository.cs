using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.RepositoryDTOs.ChatRepository;
using ChatAPI.Application.RepositoryDTOs.ChatRepository.GetListAllChats;

namespace ChatAPI.Application.Contracts
{
    public interface IChatRepository
    {
        public Task<APIResponse<Guid>> CreateChatAsync(CreateChatDTO.Request request);
        public Task<APIResponse<List<ChatDTO>>> GetListAllChatsAsync(Guid userId);
    }
}
