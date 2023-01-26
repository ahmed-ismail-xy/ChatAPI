using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.RepositoryDTOs.AuthRepository;

namespace ChatAPI.Application.Contracts
{
    public interface IAuthRepository
    {
        public Task<APIResponse<LoginDTO.Response>> RegisterUserAsync(RegisterDTO.Request request);
        public Task<APIResponse<LoginDTO.Response>> LoginUserAsync(LoginDTO.Request request);
    }
}
