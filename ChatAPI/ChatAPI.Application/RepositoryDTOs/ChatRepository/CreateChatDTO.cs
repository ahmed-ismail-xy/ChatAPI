namespace ChatAPI.Application.RepositoryDTOs.ChatRepository
{
    public class CreateChatDTO
    {
        public class Request
        {
            public Guid ReceiverId { get; set; }
            public Guid SenderId { get; set; }
        }
    }
}
