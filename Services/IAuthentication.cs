using System.Net;
using System.Threading.Tasks;
using AlgorithmEasy.Server.UserCenter.Statuses;
using AlgorithmEasy.Shared.Requests;
using AlgorithmEasy.Shared.Responses;

namespace AlgorithmEasy.Server.UserCenter.Services
{
    public interface IAuthentication
    {
        Task<RegisterStatus> Register(RegisterRequest request);

        Task<LoginResponse> Login(string userId, string password, IPAddress ip);
    }
}