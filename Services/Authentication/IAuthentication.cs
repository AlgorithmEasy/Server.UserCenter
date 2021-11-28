using AlgorithmEasy.Shared.Data;
using AlgorithmEasy.Shared.Models;

namespace AlgorithmEasy.Server.UserCenter.Services.Authentication
{
    public interface IAuthentication
    {
        RegisterResult Register(User newUser);

        LoginResult Login(string userId, byte[] password, string ip);
    }
}