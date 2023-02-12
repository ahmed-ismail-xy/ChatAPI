namespace ChatAPI.Domain.Entities
{
    public class GroupMember
    {
        public Guid GroupMemberId { get; set; }

        public Guid MemberId { get; set; }
        public bool IsAdmin { get; set; }
        public Guid GroupId { get; set; }
    }
}
