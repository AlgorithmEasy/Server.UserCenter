using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AlgorithmEasy.Server.UserCenter.Services;
using AlgorithmEasy.Server.UserCenter.Statuses;
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
        public async Task<ActionResult> Register([Required][FromBody] RegisterRequest request)
        {
            switch (await _authentication.Register(request))
            {
                case RegisterStatus.ConflictUserId:
                    return BadRequest($"用户名{request.UserId}已存在。");
                case RegisterStatus.RoleUnsupported:
                    return BadRequest("暂不支持学生以外的用户注册。");
                default:
                    return Ok();
            }
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var result = await _authentication.Login(request.UserId, request.Password,
                Request.HttpContext.Connection.RemoteIpAddress);
            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}