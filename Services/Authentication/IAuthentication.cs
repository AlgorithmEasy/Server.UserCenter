using AlgorithmEasy.Shared.Models;
using AlgorithmEasy.Shared.Responses;

namespace AlgorithmEasy.Server.UserCenter.Services.Authentication
{
    public interface IAuthentication
    {
        RegisterResponse Register(User newUser);

        LoginResponse Login(string userId, byte[] password, string ip);
    }
}