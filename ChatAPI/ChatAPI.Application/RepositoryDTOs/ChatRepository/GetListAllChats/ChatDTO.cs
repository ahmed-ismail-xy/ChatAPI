namespace ChatAPI.Application.RepositoryDTOs.ChatRepository.GetListAllChats
{
    public class ChatDTO
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string FireToken { get; set; }
        public string ChatName { get; set; }
        public string ChatImage { get; set; }
        public LastMessageDto LastMessageDto { get; set; }
    }
}
