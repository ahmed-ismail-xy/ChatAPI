namespace ChatAPI.Domain.Entities
{
    public class ChatMember
    {
        public Guid ChatMemberId { get; set; }

        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }

        public Chat Chat { get; set; }
    }
}
