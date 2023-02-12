namespace ChatAPI.Domain.Entities
{
    public class GroupMessage
    {
        public Guid GroupMessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageTime { get; set; }
        public string MessageTimeString { get; set; }
        public bool IsStarred { get; set; }

        public Guid SenderId { get; set; }
        
        public Guid GroupId { get; set; }
        
        public Guid GroupMessageStateId { get; set; }
        
        public Guid GroupMessageTypeId { get; set; }
        
        public List<GroupFilesList>? GroupFilesLists { get; set; }
    }
}
