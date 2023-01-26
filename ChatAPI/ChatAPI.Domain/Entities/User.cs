namespace ChatAPI.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? UserImage { get; set; }
        public string? Bio { get; set; }
        public string? FireToken { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? DeleteAt { get; set; }

        public UserPrivacy UserPrivacy { get; set; }
        public Guid UserPrivacyId { get; set; }
        //   public ICollection<ChatMember> ChatMembers { get; set; }
        public ICollection<BlockedContact> BlockedContacts { get; set; }

    }
}
