namespace ChatAPI.Application.RepositoryDTOs.UserRepository
{
    public class GetUserDTO
    {
        public class Response
        {
            public Guid UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string? Bio { get; set; }
            public string? FireToken { get; set; }
            public string UserImage { get; set; }
            public DateTime CreateAt { get; set; }

            public Guid UserPrivacyId { get; set; }
        }
           
    }
}
