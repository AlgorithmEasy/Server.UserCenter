using System;
using AlgorithmEasy.Shared.Data;
using AlgorithmEasy.Shared.Models;

namespace AlgorithmEasy.Server.UserCenter.Services.Authentication
{
    public class Backdoor : IAuthentication
    {
        public RegisterResult Register(User newUser)
        {
            return new RegisterResult
            {
                IsSuccess = true,
                UserId = newUser.UserId
            };
        }

        public LoginResult Login(string userId, byte[] password, string ip)
        {
            return new LoginResult
            {
                IsAuthenticated = true,
                UserId = userId,
                SessionId = new Guid()
            };
        }
    }
}