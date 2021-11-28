using AlgorithmEasy.Server.UserCenter.Services.Authentication;
using AlgorithmEasy.Shared.Data;
using AlgorithmEasy.Shared.Models;
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
        public ActionResult<RegisterResult> Register(User newUser)
        {
            var result = _authentication.Register(newUser);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<LoginResult> Login(LoginRequest request)
        {
            var result = _authentication.Login(request.UserId, request.Password,
                Request.HttpContext.Connection.RemoteIpAddress?.ToString());
            if (!result.IsAuthenticated)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}