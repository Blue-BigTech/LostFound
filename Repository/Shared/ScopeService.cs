using Microsoft.Extensions.DependencyInjection;

namespace Repository
{
    public static class ScopeService
    {
        public static IServiceCollection AddScoped(this IServiceCollection services)
        {
            services.AddScoped<IDashBoard, DashBoardService>();
            services.AddScoped<IAuth, AuthService>();
            services.AddScoped<IUserManagement, UserManagementService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAllPhotos, AllPhotosService>();

            return services;
        }
    }
}