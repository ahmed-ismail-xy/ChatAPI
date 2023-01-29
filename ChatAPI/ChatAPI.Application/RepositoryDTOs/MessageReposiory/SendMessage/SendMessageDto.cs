using ChatAPI.Domain.Entities;

namespace ChatAPI.Application.RepositoryDTOs.MessageReposiory.SendMessage
{
    public class SendMessageDto
    {
        public class Request
        {
            public string MessageText { get; set; }
            public DateTime MessageTime { get; set; }
            public string MessageTimeString { get; set; }
            public bool IsStarred { get; set; }
            public Guid SenderId { get; set; }
            public Guid ReceiverId { get; set; }
            public Guid ChatId { get; set; }
            public Guid MessageStateId { get; set; }
            public Guid MessageTypeId { get; set; }
            public List<FileListDTO>? FileLists { get; set; }
        }
        public class Response
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
}
