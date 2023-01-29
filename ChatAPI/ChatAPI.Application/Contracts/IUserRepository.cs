using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.RepositoryDTOs.UserRepository;
using ChatAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ChatAPI.Application.Contracts
{
    public interface IUserRepository
    {
        public Task<APIResponse<GetUserDTO.Response>> GetUserAsync(Guid userId);
        public Task<APIResponse<List<GetUserDTO.Response>>> GetListAllUsersAsync(Guid userId);
        public Task<APIResponse<UpdateUserDTO.Response>> UpdateUserAsync(Guid userId, UpdateUserDTO.Request request);
        public Task<APIResponse<string>> UpdateUserProfileImageAsync(Guid userId, IFormFile userImage);
        public Task<APIResponse> DeleteUserProfileImageAsync(Guid userId);
    }
}
