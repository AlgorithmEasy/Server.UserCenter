using System;
using AlgorithmEasy.Shared.Models;
using AlgorithmEasy.Shared.Response;

namespace AlgorithmEasy.Server.UserCenter.Services.Authentication
{
    public class Backdoor : IAuthentication
    {
        public RegisterResponse Register(User newUser)
        {
            return new RegisterResponse
            {
                IsSuccess = true,
                UserId = newUser.UserId
            };
        }

        public LoginResponse Login(string userId, byte[] password, string ip)
        {
            return new LoginResponse
            {
                IsAuthenticated = true,
                UserId = userId,
                SessionId = new Guid()
            };
        }
    }
}