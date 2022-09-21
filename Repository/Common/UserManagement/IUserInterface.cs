
namespace Repository
{
    public interface IUserManagement
    {
        Task AddUserRole(AppUser user, string role);
        Task<AppUser> AddNewUser(AppUser user, string password);
        Task<AppUser> FindByName(string name);
        Task<bool> DoUserExists(string userName);
        Task<AppUser> GetUserById(string userId);
        Task UpdateUser(AppUser user);
        Task<bool> DoRoleExists(AppUser user, string role);
        Task AccessFailed(AppUser user);
        Task<bool> UserIsLockedOut(AppUser user);
        Task<bool> CheckUserPassword(AppUser user, string password);
        Task ResetAccessFailedCount(AppUser user);
        //Task<bool> ResetPasswordAsync(AppUser user, string token, string password);
        //Task<bool> UpdatePasswordAsync(AppUser user, string password, string newPassword);
    }
}
