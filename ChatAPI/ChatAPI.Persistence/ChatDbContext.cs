using ChatAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Persistence
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<BlockedContact> blockedContacts { get; set; }
        public DbSet<ChatMember> ChatMembers { get; set; }
        public DbSet<FileList> FileLists { get; set; }
        public DbSet<LastSeenPrivacy> LastSeenPrivacies { get; set; }
        public DbSet<MessageState> MessageStates { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<ProfileImagePrivacy> ProfileImagePrivacies { get; set; }
        public DbSet<UserPrivacy> UserPrivacies { get; set; }
    }
}
