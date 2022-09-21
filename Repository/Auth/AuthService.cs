using System.Net;
namespace Repository
{
    public class AuthService : IAuth
    {
        private Context _db { get; }
        private Jwt _jwt { get; }
        private IUserManagement _userManagement;
        public AuthService(IOptions<Jwt> jwt, Context db, IUserManagement userManagement)
        {
            _jwt = jwt.Value;
            _db = db;
            _userManagement = userManagement;
        }
        public async Task<Response> SignIn(LoginModel model)
        {
            Response response = new();


            if (!(await _userManagement.DoUserExists(model.UserName)))
            {
                throw new ApplicationException(Message.InvalidUserName);
            }
            var user = await _userManagement.FindByName(model.UserName);
            if (await _userManagement.UserIsLockedOut(user))
            {
                throw new ApplicationException(Message.UserLogedOut);
            }
            if (await _userManagement.CheckUserPassword(user, model.UserPassword))
            {
                await _userManagement.ResetAccessFailedCount(user);
            }
            else
            {
                await _userManagement.AccessFailed(user);
                if (await _userManagement.UserIsLockedOut(user))
                {
                    throw new ApplicationException(Message.TooManyLoginAlert);
                }
                throw new ApplicationException(Message.InvalidUserName);
            }
            if ((model.IsAdmin && (await _userManagement.DoRoleExists(user, RolesList.Admin.ToString()))) ||
                (!model.IsAdmin && (await _userManagement.DoRoleExists(user, RolesList.User.ToString()))))
            {
                response.Data = (new
                {
                    Token = _jwt.GetToken(user.Id),
                    UserId = user.Id.Encode(),
                    UserName = user.UserName,
                    PoliceStation = user.PoliceStation + ", " + user.DistrictCorporation
                }).Serialize();
            }
            else
            {
                throw new ApplicationException("access not allowed");
            }
            return response;
        }
        public async Task<Response> GetTokenValid(string token)
        {
            Response response = new();
            //var user = await _userManagement.GetUserById(_jwt.LoggedUserId());
            var isValidate = token.ValidateToken(_jwt);
            if (isValidate.IsValidated && isValidate.StatusCode != (int)HttpStatusCode.Redirect && isValidate.StatusCode != (int)HttpStatusCode.Unauthorized)
            {
                response.StatusCode = (int)HttpStatusCode.Accepted;
                //response.Token = token;
            }
            return response;
        }
    }
}
