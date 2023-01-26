namespace ChatAPI.Domain.Entities
{
    public class LastSeenPrivacy
    {
        public Guid LastSeenPrivacyId { get; set; }
        public string LastSeenState { get; set; }

        public UserPrivacy UserPrivacy { get; set; }
    }
}
