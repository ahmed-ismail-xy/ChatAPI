namespace ChatAPI.Domain.Entities
{
    public class MessageState
    {
        public Guid MessageStateId { get; set; }
        public string State { get; set; }

        public Message Message { get; set; }
    }
}
