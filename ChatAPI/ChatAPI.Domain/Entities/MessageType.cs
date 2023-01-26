namespace ChatAPI.Domain.Entities
{
    public class MessageType
    {
        public Guid MessageTypeId { get; set; }
        public string MessageTypeName { get; set; }
        public Message Message { get; set; }

    }
}
