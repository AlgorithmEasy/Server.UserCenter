using System.Net;
using System.Threading.Tasks;
using AlgorithmEasy.Server.UserCenter.Statuses;
using AlgorithmEasy.Shared.Requests;
using AlgorithmEasy.Shared.Responses;

namespace AlgorithmEasy.Server.UserCenter.Services.Authentication
{
    public class Backdoor : IAuthentication
    {
        public Task<RegisterStatus> Register(RegisterRequest request)
        {
            return Task.FromResult(RegisterStatus.Success);
        }

        public Task<LoginResponse> Login(string userId, string password, IPAddress ip)
        {
            return Task.FromResult(new LoginResponse
            {
                UserId = userId
            });
        }
    }
}