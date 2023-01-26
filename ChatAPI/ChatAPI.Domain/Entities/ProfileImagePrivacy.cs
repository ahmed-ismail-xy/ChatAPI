namespace ChatAPI.Domain.Entities
{
    public class ProfileImagePrivacy
    {
        public Guid ProfileImagePrivacyId { get; set; }
        public string ImagePrivacyState { get; set; }

        public UserPrivacy UserPrivacy { get; set; }
    }
}
