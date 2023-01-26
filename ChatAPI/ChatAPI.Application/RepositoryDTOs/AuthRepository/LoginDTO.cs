namespace ChatAPI.Application.RepositoryDTOs.AuthRepository
{
    public class LoginDTO
    {
        public class Request
        {
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }
        public class Response
        {
            public string Token { get; set; }
            public DateTime ExpireDate { get; set; }
            public Guid UserId { get; set; }
        }
    }
}
