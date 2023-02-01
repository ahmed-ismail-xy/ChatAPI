namespace ChatAPI.Domain.Entities
{
    public class GroupFilesList
    {
        public Guid GroupFilesListId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public bool IsRecord { get; set; }
        public Guid GroupMessageId { get; set; }
    }
}
