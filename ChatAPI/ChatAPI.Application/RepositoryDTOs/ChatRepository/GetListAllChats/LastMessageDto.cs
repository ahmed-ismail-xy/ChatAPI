using ChatAPI.Domain.Entities;

namespace ChatAPI.Application.RepositoryDTOs.ChatRepository.GetListAllChats
{
    public class LastMessageDto
    {
        public Guid MessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageTime { get; set; }
        public string MessageTimeString { get; set; }
        public Guid SenderId { get; set; }

        public Guid MessageStateId { get; set; }
        public Guid MessageTypeId { get; set; }
    }
}
