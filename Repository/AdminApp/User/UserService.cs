global using AutoMapper;
namespace Repository
{
    public class UserService : IUserService
    {
        private Context db;
        private IUserManagement _userManagement;
        private readonly IMapper _mapper;
        public UserService(Context _db, IUserManagement userManagement, IMapper mapper)
        {
            this.db = _db;
            _userManagement = userManagement;
            _mapper = mapper;
        }
        public async Task<Response> SaveUser(UserModel model)
        {
            Response response = new Response();
            if (model is null)
                throw new ApplicationException(Message.ErrorMessage);
            AppUser appUser = _mapper.Map<AppUser>(model);
            appUser.CreatedDate = NodaTimeHelper.Now;
            appUser = await _userManagement.AddNewUser(appUser, model.UserPassword);
            if (appUser is null)
                throw new ApplicationException(Message.ErrorMessage);

            await _userManagement.AddUserRole(appUser, RolesList.User.ToString());
            response.Detail = Message.GenerateUserMessage;
            return response;
        }
        public async Task<Response> FetchUser(Pagination pages)
        {
            Response response = new Response();

            var userData = await db.LoadStoredProcedure("[dbo].[FetchUser]")
                                   .WithSqlParams(("PageSize", pages.PageSize),
                                                  ("PageNum", pages.PageNum),
                                                  ("SortCol", pages.SortCol),
                                                  ("SortOrder", pages.SortOrder),
                                                  ("AspNetRoleId", pages.AspNetRoleId))
                                                  .ExecuteStoredProcedureAsync<UserModelDto>();
            if (!userData.Any())
                throw new ApplicationException(Message.ErrorMessage);
            response.Data = userData.Serialize();
            response.Total = userData[0].Total;
            return response;
        }

        public async Task<Response> ResetPassword(ResetViewModel model)
        {
            Response response = new Response();
            AppUser appuser = await _userManagement.GetUserById(model.Code.Decode());
            if (appuser is null)
                throw new ApplicationException(Message.ErrorMessage);

            appuser.UserPassword = model.UserPassword;
            await _userManagement.UpdateUser(appuser);
            response.Detail = Message.ResetPasswordMessage;
            return response;
        }


        public async Task<Response> GetLoginUser(string Code)
        {
            Response response = new Response();
            AppUser appuser = await _userManagement.GetUserById(Code.Decode());
            if (appuser is null)
                throw new ApplicationException(Message.ErrorMessage);

            response.Data = new
            {
                UserName = appuser.UserName,
                PoliceStationCorportion = appuser.PoliceStation + ", " + appuser.DistrictCorporation
            }.Serialize();
            return response;
        }
    }
}
