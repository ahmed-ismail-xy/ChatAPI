namespace ChatAPI.Domain.Entities
{
    public class FileList
    {
        public Guid FileListId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public bool IsRecord { get; set; }
        public Guid MessageId { get; set; }
    }
}
