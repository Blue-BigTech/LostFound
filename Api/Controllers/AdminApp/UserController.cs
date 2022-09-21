
namespace ImageRecongnitionApi.Controllers.AdminApp
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/user")]
    public class UserController : ControllerBase
    {
        private IUserService _user;
        public UserController(IUserService user)
        {
            _user = user;
        }
        [HttpPost("save")]
        public async Task<IActionResult> SaveUser(UserModel model)
        {
            return (await _user.SaveUser(model)).Format(this);
        }

        [HttpPost("get")]
        public async Task<IActionResult> FetchUser(Pagination pages)
        {
            return (await _user.FetchUser(pages)).Format(this);
        }
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetViewModel model)
        {
            return (await _user.ResetPassword(model)).Format(this);
        }

        [HttpGet("getloginuser")]
        public async Task<IActionResult> GetLoginUser(string Code)
        {
            return (await _user.GetLoginUser(Code)).Format(this);
        }
    }
}
