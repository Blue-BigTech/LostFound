
namespace Repository
{

    public interface IUserService
    {
        Task<Response> SaveUser(UserModel model);
        Task<Response> FetchUser(Pagination pages);
        Task<Response> ResetPassword(ResetViewModel model);
        Task<Response> GetLoginUser(string Code);
    }
}
