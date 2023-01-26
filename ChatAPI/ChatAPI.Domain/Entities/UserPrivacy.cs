namespace ChatAPI.Domain.Entities
{
    public class UserPrivacy
    {
        public Guid UserPrivacyId { get; set; }

        public LastSeenPrivacy LastSeenPrivacy { get; set; }
        public Guid LastSeenPrivacyId { get; set; }

        public ProfileImagePrivacy ProfileImagePrivacy { get; set; }
        public Guid ProfileImagePrivacyId { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
