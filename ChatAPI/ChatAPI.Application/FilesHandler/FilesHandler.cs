using Microsoft.AspNetCore.Http;

namespace ChatAPI.Application.FilesHandler
{
    public static class FileHandler
    {
        private static readonly string _rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        private static readonly string _userProfileImageDirectory = Path.Combine(_rootDirectory, @"chatmedia\userprofileimage");
        private static readonly string _chatFilesDirectory = Path.Combine(_rootDirectory, @"chatmedia\chats");

        public static string UpdateUserProfileImage(Guid userId, IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);
            string uniqueFileName = userId + fileExtension;
            string imagePath = Path.Combine(_userProfileImageDirectory, uniqueFileName);
            
            EnsureDirectoryExists(_userProfileImageDirectory);

            WriteCred();
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            imagePath = Path.Combine(@"chatmedia\userprofileimage", uniqueFileName);
            return imagePath;
        }

        public static void DeleteUserProfileImage(string imagePath)
        {
            string fullPath = Path.Combine(_rootDirectory, imagePath);
            File.Delete(fullPath);
        }

        public static async Task<string> HandelChatsFiles(IFormFile file, Guid chatId)
        {
            string chatFilesDirectory = Path.Combine(_chatFilesDirectory, chatId.ToString(), "files");
            string imagePath = Path.Combine(chatFilesDirectory, file.FileName);

            EnsureDirectoryExists(chatFilesDirectory);

            WriteCred();
            await using var fileStream = new FileStream(imagePath, FileMode.Create);
            file.CopyTo(fileStream);

            imagePath = Path.Combine(@"chatmedia\chats", chatId.ToString(), @"files", file.FileName);
            return imagePath;
        }
        private static void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private static void WriteCred()
        {
            CredentialManager.WriteCred();
        }
    }
}
