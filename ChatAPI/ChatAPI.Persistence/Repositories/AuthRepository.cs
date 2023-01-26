using AutoMapper;
using ChatAPI.Application.Contracts;
using ChatAPI.Application.Featuers.ResponseHandler;
using ChatAPI.Application.RepositoryDTOs.AuthRepository;
using ChatAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public AuthRepository(ChatDbContext dbContext, IConfiguration configuration, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<APIResponse<LoginDTO.Response>> RegisterUserAsync(RegisterDTO.Request request)
        {
            APIResponse<LoginDTO.Response> response = new APIResponse<LoginDTO.Response>();

            var user = _dbContext.Users.AsNoTracking().Where(U => U.PhoneNumber == request.PhoneNumber).FirstOrDefault();

            if (user is not null)
            {
                response.AddError(3);
                response.Message = "User Already Exist";

                return response;
            }

            var newUser = _mapper.Map<User>(request);

            newUser.UserPrivacyId = Guid.Parse("2ed2f3de-bda8-49af-86e3-51d08ead5f4a");
            newUser.CreateAt = DateTime.Now;

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            LoginDTO.Request login = new LoginDTO.Request() { Password = newUser.Password, PhoneNumber = newUser.PhoneNumber };

            return await LoginUserAsync(login);
        }
        public async Task<APIResponse<LoginDTO.Response>> LoginUserAsync(LoginDTO.Request request)
        {
            APIResponse<LoginDTO.Response> response = new APIResponse<LoginDTO.Response>();

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
    }
}
