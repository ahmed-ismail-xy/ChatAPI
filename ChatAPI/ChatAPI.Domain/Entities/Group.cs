namespace ChatAPI.Domain.Entities
{
    public class Group
    {
        public Guid GroupId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string GroupName { get; set; }
        public string GroupImage { get; set; }
        public string GroupBio { get; set; }
        public Guid GroupSettingId { get; set; }
        public ICollection<GroupMember> GroupMembers { get; set; }
        public ICollection<GroupMessage> GroupMessages { get; set; }
    }
}
