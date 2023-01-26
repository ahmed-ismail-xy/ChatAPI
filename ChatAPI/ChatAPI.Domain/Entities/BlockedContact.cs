namespace ChatAPI.Domain.Entities
{
    public class BlockedContact
    {
        public Guid BlockedContactId { get; set; }
        public DateTime BlockedAt { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
