namespace ChatAPI.Domain.Entities
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageTime { get; set; }
        public string MessageTimeString { get; set; }
        public bool IsStarred { get; set; }
        public Guid SenderId { get; set; }
        public Guid ChatId { get; set; }
        public Guid MessageStateId { get; set; }
        public Guid MessageTypeId { get; set; }
        public List<FileList>? FileLists { get; set; }
    }
}
