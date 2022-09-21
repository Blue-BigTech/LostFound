
namespace Repository
{
    public interface IAuth
    {
        Task<Response> SignIn(LoginModel user);
        Task<Response> GetTokenValid(string token);

    }
}
