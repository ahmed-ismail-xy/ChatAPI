using AutoMapper;
using ChatAPI.Application.Contracts;
using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.FilesHandler;
using ChatAPI.Application.RepositoryDTOs.UserRepository;
using ChatAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ChatAPI.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly ChatDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ChatDbContext dbContext, IMapper mapper, ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<APIResponse<List<GetUserDTO.Response>>> GetListAllUsersAsync(Guid userId)
        {
            var response = new APIResponse<List<GetUserDTO.Response>>();
            try
            {
                var allUsers = _dbContext.Users.AsNoTracking().Where(U => U.UserId != userId).ToList();
                var allUsersDto = _mapper.Map<List<GetUserDTO.Response>>(allUsers);

                response.Data = allUsersDto;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.Message = "Unexpected Error Occurred.";
                response.AddError(10);
                return response;
            }
        }
        public async Task<APIResponse<GetUserDTO.Response>> GetUserAsync(Guid userId)
        {
            APIResponse<GetUserDTO.Response> response = new APIResponse<GetUserDTO.Response>();
            try
            {
                var user = await _dbContext.Users.AsTracking().FirstOrDefaultAsync(U => U.UserId == userId);
                if (user == null)
                {
                    response.AddError(5);
                    response.Message = "User Not Found.";
                    return response;
                }
                var userDto = _mapper.Map<GetUserDTO.Response>(user);
                response.Data = userDto;

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.Message = "Unexpected Error Occurred.";
                response.AddError(10);
                return response;
            }
        }
        public async Task<APIResponse<UpdateUserDTO.Response>> UpdateUserAsync(Guid userId, UpdateUserDTO.Request request)
        {

            var result = new APIResponse<UpdateUserDTO.Response>();
            try
            {
                var user = await _dbContext.Users.AsTracking().FirstOrDefaultAsync(U => U.UserId == userId);
                if (user == null)
                {
                    result.AddError(5);
                    result.Message = "User Not Found.";
                    return result;
                }

                var responses = new UpdateUserDTO.Response();
                if (!request.Password.IsNullOrEmpty())
                {
                    user.Password = request.Password;
                    responses.APIResponses.Add(new APIResponse { Message = "Password Updated" });
                }

                if (!request.Bio.IsNullOrEmpty())
                {
                    user.Bio = request.Bio;
                    responses.APIResponses.Add(new APIResponse { Message = "Bio Updated" });
                }

                if (!request.FirstName.IsNullOrEmpty())
                {
                    user.FirstName = request.FirstName;
                    responses.APIResponses.Add(new APIResponse { Message = "First Name Updated" });
                }

                if (!request.LastName.IsNullOrEmpty())
                {
                    user.LastName = request.LastName;
                    responses.APIResponses.Add(new APIResponse { Message = "Last Name Updated" });
                }

                if (!request.FireToken.IsNullOrEmpty())
                {
                    user.FireToken = request.FireToken;
                    responses.APIResponses.Add(new APIResponse { Message = "FireToken Updated" });
                }

                if (request.UserPrivacyId != null)
                {
                    user.UserPrivacyId = (Guid)request.UserPrivacyId;
                    responses.APIResponses.Add(new APIResponse { Message = "User Privacy Updated" });
                }

                await _dbContext.SaveChangesAsync();
                result.Data = responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation("Request INFO:", request.ToString());
                result.Message = "Unexpected Error Occurred.";
                result.AddError(10);
                return result;
            }

            return result;
        }
        public async Task<APIResponse<string>> UpdateUserProfileImageAsync(Guid userId, IFormFile userImage)
        {
            var response = new APIResponse<string>();
            try
            {
                var user = await _dbContext.Users.AsTracking().FirstOrDefaultAsync(U => U.UserId == userId);

                if (user == null)
                {
                  response.Message = "User Not Found.";
                  response.AddError(5);
                    return response;
                }

                if (userImage == null)
                {
                    response.Message = "No Image Provided.";
                    response.AddError(7);
                    return response;
                }

                if (!string.IsNullOrEmpty(user.UserImage))
                {
                    try
                    {
                        FileHandler.DeleteUserProfileImage(user.UserImage);
                    }
                    catch (Exception ex)
                    {

                        response.Message = "Failed to Delete Previous Image.";
                        response.AddError(8);
                        return response;
                    }
                }

                try
                {
                    string imagePath = FileHandler.UpdateUserProfileImage(userId, userImage);
                    user.UserImage = @$"{imagePath}";
                    await _dbContext.SaveChangesAsync();

                    return new APIResponse<string> { Data = @$"{imagePath}" };
                }
                catch (Exception ex)
                {
                    response.Message = "Failed to Update Profile Image.";
                    response.AddError(9);
                    return response;                 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.Message = "Unexpected Error Occurred.";
                response.AddError(10);
                return response;
            }
        }
        public async Task<APIResponse> DeleteUserProfileImageAsync(Guid userId)
        {
            var response = new APIResponse();
            try
            {
                var user = await _dbContext.Users.AsTracking().FirstOrDefaultAsync(U => U.UserId == userId);

                if (user == null)
                {

                    response.Message = "User Not Found.";
                    response.AddError(5);
                }

                try
                {
                    if (!string.IsNullOrEmpty(user.UserImage))
                    {
                        FileHandler.DeleteUserProfileImage(user.UserImage);
                    }
                }
                catch (Exception ex)
                {
                    response.Message = "Failed to Delete User Profile Image.";
                    response.AddError(11);
                    return response;
                }

                string imagePath = Path.Combine(@"chatmedia\userprofileimage", "DefaultProfile.png");

                user.UserImage = imagePath;
                await _dbContext.SaveChangesAsync();

                return new APIResponse { Message = "User Profile Image Deleted" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.Message = "Unexpected Error Occurred.";
                response.AddError(10);
                return response;
            }
        }
    }
}