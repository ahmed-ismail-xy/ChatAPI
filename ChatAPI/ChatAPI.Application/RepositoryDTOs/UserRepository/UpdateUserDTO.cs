using ChatAPI.Application.Featuers.ResponseHandler;

namespace ChatAPI.Application.RepositoryDTOs.UserRepository
{
    public class UpdateUserDTO
    {
        public class Request
        {
            public string? FirstName { get; set; } = null;
            public string? LastName { get; set; } = null;
            public string? Password { get; set; } = null;
            public string? Bio { get; set; } = null;
            public string? FireToken { get; set; } = null;
            public string? UserImage { get; set; } = null;
            public Guid? UserPrivacyId { get; set; } = null;

        }
        public class Response
        {
            public List<APIResponse> APIResponses = new List<APIResponse>();
        }
    }
}
