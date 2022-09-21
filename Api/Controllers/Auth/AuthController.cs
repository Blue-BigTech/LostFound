global using Common.Abstract.Entities;
global using Common.Abstract.Extension;
global using Repository;
global using Microsoft.AspNetCore.Mvc;
global using Common.Dto;

namespace ImageRecongnitionApi.Controllers.AdminApp
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/auth")]

    public class AuthController : ControllerBase
    {
        private IAuth _authRepository;
        public AuthController(IAuth authRepository)
        {
            _authRepository = authRepository;
        }
            
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginModel user)
        {
            return (await _authRepository.SignIn(user)).Format(this);
        }
        [HttpGet("gettokenvalid")]
        public async Task<Response> GetTokenValid(string token)
        {
            return await _authRepository.GetTokenValid(token);
        }
    }
}
