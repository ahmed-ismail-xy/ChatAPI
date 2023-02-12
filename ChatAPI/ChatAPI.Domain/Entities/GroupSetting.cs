namespace ChatAPI.Domain.Entities
{
    public class GroupSetting
    {
        public Guid GroupSettingId { get; set; }
        public bool IsNotifactionsMuted { get; set; }
        public bool IsOnlyAdminSend { get; set; }
    }
}
