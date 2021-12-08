using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AlgorithmEasy.Server.UserCenter.Statuses;
using AlgorithmEasy.Shared.Models;
using AlgorithmEasy.Shared.Requests;
using AlgorithmEasy.Shared.Responses;
using AlgorithmEasy.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AlgorithmEasy.Server.UserCenter.Services.Authentication
{
    public class ProductAuthentication : IAuthentication
    {
        private readonly AlgorithmEasyDbContext _dbContext;

        public ProductAuthentication(AlgorithmEasyDbContext dbContext) => _dbContext = dbContext;

        public async Task<RegisterStatus> Register(RegisterRequest request)
        {
            if (_dbContext.Users.Any(u => u.UserId == request.UserId))
                return RegisterStatus.ConflictUserId;

            var role = _dbContext.Roles.SingleOrDefault(r => r.RoleId == request.RoleId);
            if (role is not { RoleName: "Student" })
                return RegisterStatus.RoleUnsupported;

            var user = new User
            {
                UserId = request.UserId,
                Password = (request.UserId + request.Password).GetSha256(),
                Role = role,
                Nickname = request.Nickname,
                Email = request.Email
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return RegisterStatus.Success;
        }

        public async Task<LoginResponse> Login(string userId, string password, IPAddress ip)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.UserId == userId && u.Password == (userId + password).GetSha256());
            if (user == null)
                return null;

            var session = new Session
            {
                SessionId = Guid.NewGuid(),
                User = user,
                Ip = ip
            };
            await _dbContext.Sessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();

            return new LoginResponse
            {
                UserId = userId,
                Role = user.Role.RoleName,
                Token = GetJsonWebToken(user)
            };
        }

        private static string GetJsonWebToken(User user)
        {
            var claim = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            }, "GetToken");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("ALGORITHMEASY_SECURITY_TOKENS_KEY")!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Environment.GetEnvironmentVariable("ALGORITHMEASY_SECURITY_TOKENS_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("ALGORITHMEASY_SECURITY_TOKENS_AUDIENCE"),
                Subject = claim,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}