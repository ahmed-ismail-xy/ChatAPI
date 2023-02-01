namespace ChatAPI.Domain.Entities
{
    public class GroupMember
    {
        public Guid GroupMemberId { get; set; }

        public User User { get; set; }
        public Guid MemberId { get; set; }

        public Group Group { get; set; }
    }
}
