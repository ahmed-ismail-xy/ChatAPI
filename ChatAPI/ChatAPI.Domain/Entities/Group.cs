namespace ChatAPI.Domain.Entities
{
    public class Group
    {
        public Guid GroupId { get; set; }
        public Guid GroupDefaultAdminId { get; set; }
        public string GroupName { get; set; }
        public string GroupImage { get; set; }
        public string GroupBio { get; set; }

        public ICollection<GroupMember> Memberships { get; set; }
        public ICollection<GroupMessage> GroupMessages { get; set; }
    }
}
