using AutoMapper;
using ChatAPI.Application.Contracts;
using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.RepositoryDTOs.AuthRepository;
using ChatAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatAPI.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        protected readonly ChatDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthRepository> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRepository(ChatDbContext dbContext, IMapper mapper, IConfiguration configuration, ILogger<AuthRepository> logger, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse<LoginDTO.Response>> RegisterUserAsync(RegisterDTO.Request request)
        {

            var response = new APIResponse<LoginDTO.Response>();

            try
            {
                var user = await _dbContext.Users
              .AsNoTracking()
              .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

                if (user != null)
                {
                    response.AddError(3);
                    response.Message = "User Already Exists";
                    return response;
                }

                var newUser = _mapper.Map<User>(request);
                newUser.UserPrivacyId = Guid.Parse("2ed2f3de-bda8-49af-86e3-51d08ead5f4a");
                newUser.CreateAt = DateTime.Now;
                string imagePath = Path.Combine(@"chatmedia\userprofileimage", "DefaultProfile.png");
                newUser.UserImage = @$"{imagePath}";
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                var login = new LoginDTO.Request { Password = newUser.Password, PhoneNumber = newUser.PhoneNumber };
                return await LoginUserAsync(login);
            }
            catch (Exception ex)
            {
                var requestId = _httpContextAccessor.HttpContext.Response.Headers["Request-Id"];
                _logger.LogError(requestId,ex.Message);
                _logger.LogInformation(requestId,": Request INFO:", request.ToString());
                response.Message = "Unexpected Error Occurred.";
                response.AddError(10);
                return response;
            }
        }
        public async Task<APIResponse<LoginDTO.Response>> LoginUserAsync(LoginDTO.Request request)
        {
            APIResponse<LoginDTO.Response> response = new APIResponse<LoginDTO.Response>();

            try
            {
                var user = _dbContext.Users.AsNoTracking().Where(U => U.PhoneNumber == request.PhoneNumber).FirstOrDefault();

                if (user is null)
                {
                    response.AddError(1);
                    response.Message = "Invalid PhoneNumber";
                    return response;
                }
                else if (!user.Password.Equals(request.Password))
                {
                    response.AddError(1);
                    response.Message = "Invalid Password";
                    return response;
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
            };
                var keyBuffer = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:SecretKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtOptions:Issuer"],
                    audience: _configuration["JwtOptions:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: new SigningCredentials(keyBuffer, SecurityAlgorithms.HmacSha256));
                string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                response.Data = new LoginDTO.Response { Token = tokenAsString, ExpireDate = token.ValidTo, UserId = user.UserId };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation("Request INFO:", request.ToString());
                response.Message = "Unexpected Error Occurred.";
                response.AddError(10);
                return response;
            }

        }

    }
}



      