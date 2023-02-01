namespace ChatAPI.Domain.Entities
{
    public class GroupMessageType
    {
        public Guid GroupMessageTypeId { get; set; }
        public string MessageTypeName { get; set; }
        public GroupMessage GroupMessage { get; set; }
    }
}
