namespace ChatAPI.Domain.Entities
{
    public class GroupMessage
    {
        public Guid GroupMessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageTime { get; set; }
        public string MessageTimeString { get; set; }
        public bool IsStarred { get; set; }

        public Guid MemberId { get; set; }
        
        public Group Group { get; set; }
        public Guid GroupId { get; set; }
        
        public GroupMessageState GroupMessageState { get; set; }
        public Guid GroupMessageStateId { get; set; }
        
        public GroupMessageType GroupMessageType { get; set; }
        public Guid GroupMessageTypeId { get; set; }
        
        public List<GroupFilesList>? GroupFilesLists { get; set; }
    }
}
