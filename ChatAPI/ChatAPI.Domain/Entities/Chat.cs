namespace ChatAPI.Domain.Entities
{
    public class Chat
    {
        public Guid ChatId { get; set; }

        public ChatMember ChatMember { get; set; }
        public Guid ChatMemberId { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}