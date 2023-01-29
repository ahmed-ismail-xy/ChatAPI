using Microsoft.AspNetCore.Http;

namespace ChatAPI.Application.RepositoryDTOs.MessageReposiory.SendMessage
{
    public class FileListDTO
    {
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public bool IsRecord { get; set; }
    }
}
