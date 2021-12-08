using AlgorithmEasy.Server.UserCenter.Services.Authentication;
using AlgorithmEasy.Shared.Models;
using AlgorithmEasy.Shared.Requests;
using AlgorithmEasy.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AlgorithmEasy.Server.UserCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authentication;

        public AuthenticationController(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        [HttpPost]
        public ActionResult<RegisterResponse> Register(User newUser)
        {
            var result = _authentication.Register(newUser);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<LoginResponse> Login(LoginRequest request)
        {
            var result = _authentication.Login(request.UserId, request.Password,
                Request.HttpContext.Connection.RemoteIpAddress?.ToString());
            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}