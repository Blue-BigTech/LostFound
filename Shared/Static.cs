
namespace Shared
{
    public static class StaticContext
    {
        public static string ApiUrl { get { return "https://image-recog-api.arhamsoft.net/api/1.0/"; } }
        //public static string ApiUrl { get { return "https://localhost:7238/api/1.0/"; } }
        public static string SignInApiUrl { get { return ApiUrl + "auth/signin"; } }
        public static string AdminDashboardApiUrl { get { return ApiUrl + "admin/save"; } }
        public static string AdminDeletePhotoApiUrl { get { return ApiUrl + "admin/delete?Code="; } }
        public static string FetchAdminPhotosApiUrl { get { return ApiUrl + "admin/get"; } }
        public static string ResetPasswordApiUrl { get { return ApiUrl +  "user/resetpassword"; } }
        public static string FetchUserApiUrl { get { return ApiUrl + "user/get"; } }
        public static string SaveUserApiUrl { get { return ApiUrl + "user/save"; } }
        public static string FetchPhotosApiUrl { get { return ApiUrl + "client/get"; } }
        public static string GetLoginUserApiUrl { get { return ApiUrl + "user/getloginuser?Code="; } }
        public static string Auth { get { return "LgAAAB_@_LCAAAAAAAAApzqDQyKfHRNLY1NbENyM80VFVRtrQ0NzYztLKwsDQ2MHIwNo6Njo830DY00tXVBgAGvJ4yLgAAAA_!__!_"; } }

        public static string UserId { get; set; }
        public static string AdminName { get; set; }
        public static string PoliceStation { get; set; }
        public static string ItemCode { get; set; }
        public static bool IsNavbarPopup { get; set; } 
        //Review: get the Id from db
        public static string AdminRoleId { get { return "b5bcbf9d-906c-4417-9953-d81c9c8caf88"; } }
        //public static string ApiUrl { get { return "https://localhost:7238/api/1.0/"; } }

    }
}
