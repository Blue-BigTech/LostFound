using Microsoft.AspNetCore.Identity;

namespace Repository
{
    public class UserManagementService : IUserManagement
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private Context _db { get; }
        public UserManagementService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, Context db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public async Task<AppUser> AddNewUser(AppUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw GetIdentityException(result);

            return user;
        }

        public async Task<AppUser> FindByName(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }
        public async Task<bool> DoRoleExists(AppUser user,string role)
        {
            var userRole = await _userManager.GetRolesAsync(user);
            return (userRole != null && userRole[0] == role) ? true : false;
        }
        public async Task AddUserRole(AppUser user, string role)
        { 
             await _userManager.AddToRoleAsync(user, role);        
        }
        public async Task<AppUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new ApplicationException($"User ({userId}) not found!");

            return user;
        }
        public async Task<bool> DoUserExists(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName.Trim());
            if (user == null)
                return false;

            return true;
        }
        public async Task UpdateUser(AppUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw GetIdentityException(result);

        }
        public async Task AccessFailed(AppUser user)
        {
            var result = await _userManager.AccessFailedAsync(user);

            if (!result.Succeeded)
                throw GetIdentityException(result);
        }
        public async Task<bool> UserIsLockedOut(AppUser user)
        {
            return await _userManager.IsLockedOutAsync(user);
        }
        public async Task<bool> CheckUserPassword(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task ResetAccessFailedCount(AppUser user)
        {
            var result = await _userManager.ResetAccessFailedCountAsync(user);

            if (!result.Succeeded)
                throw GetIdentityException(result);
        }
        //public async Task<bool> ResetPasswordAsync(AppUser user, string token, string password)
        //{
        //    var result = await _userManager.ResetPasswordAsync(user, token, password);
        //    if (!result.Succeeded)
        //        return false;
        //    else
        //        return true;

        //}
        //public async Task<bool> UpdatePasswordAsync(AppUser user, string password, string newPassword)
        //{
        //    //var result = _userManager.PasswordHasher.HashPassword(user, password);
        //    var result = await _userManager.ChangePasswordAsync(user, password, newPassword);
        //    if (result == null)
        //        return false;

        //    return true;
        //}

        private Exception GetIdentityException(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(x => x.Description);
                var formattedErrors = string.Join(Environment.NewLine, errors);
                return new ApplicationException(formattedErrors);
            }
            else
            {
                return new ApplicationException("Identity succeeded, not an TranslationException");
            }
        }

    }
}
