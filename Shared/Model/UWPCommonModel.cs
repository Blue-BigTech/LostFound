using Realms;
using System;
using System.Net;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Shared.Model
{
    public class AdminLoginModel
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class DasboardModel
    {
        public bool IsLost { get; set; }
        public string Name { get; set; }
        public string CPName { get; set; }
        public string CPPhoneNo { get; set; }
        public string GDCaseNo { get; set; }
        public string OfficerName { get; set; }
        public string OfficerBPNo { get; set; }
        public string OfficerPhoneNo { get; set; }
        public string AspNetUserId   { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public byte[] Image { get; set; }
    }
    public class UserModel : AdminLoginModel
    {
        public string DistrictCorporation { get; set; }
        public string PoliceStation { get; set; }
    }
    public class ResponseBody
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public dynamic Data { get; set; }
        public long Total { get; set; }
        public bool IsValidated { get { return StatusCode == (int)HttpStatusCode.OK; } }
    }
    public class PasswordOptions
    {
        public int RequiredLength { get; set; }
        public int RequiredUniqueChars { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool RequireUppercase { get; set; }
    }
    public class Page
    {
        //public int PageSize = 120;
        public int PageSize = 5;
        public int PageNum = 1;
        public int SortCol = 0;
        public string SortOrder = "desc";
        public string AspNetRoleId = StaticContext.AdminRoleId;
    }

    public class ClientPagination
    {
        public int PageSize = 12;
        public int PageNum = 1;
        public string AspNetUserId = StaticContext.UserId;
    }
    public class UserResponseModel
    {

        public string Code { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string PoliceStation { get; set; }
        public int Total { get; set; }

    }
    public class UserResponseOnLoad
    {
        public string UserName { get; set; }
        public string PoliceStationCorportion { get; set; }

    }

    public class LoginModel : RealmObject
    {
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset LoginTime { get; set; }


    }

    public class ResetPassword 
    {
        public string UserPassword { get; set; }
        public string Code { get; set; }



    }
    public class UserModal
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PoliceStation { get; set; }



    }
    public class ResponseSignIn : RealmObject
    {
        //public HttpStatusCode HttpCode { get; set; } /*= HttpStatusCode.OK;*/
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string detail { get; set; }
        public bool IsValidated { get { return StatusCode == (int)HttpStatusCode.OK; } }
    }

    public class AdminPagination
    {
        public int PageSize = 12;
        public int PageNum = 1;
        public string AspNetUserId = null;
    }

    public class AllPhotosResponseModel
    {
        public string Code { get; set; }
        //Review :Why Int64 use int
        public Int64 RowNo { get; set; }
        public string Name { get; set; }
        public string CPName { get; set; }
        public string CPPhoneNo { get; set; }
        public string GDCaseNo { get; set; }
        public string OfficerName { get; set; }
        public string OfficerBPNo { get; set; }
        public string OfficerPhoneNo { get; set; }
        public string Remarks { get; set; }
        public string ImageURL { get; set; }

        public string IsLost { get; set; }
        //Donot hard code strings make enum
        public string Islost
        {
            get
            {
                return (IsLost == "Found Person") ?  new SolidColorBrush(Colors.Green).ToString() : new SolidColorBrush(Colors.Red).ToString();
            }
        }
        public string CreatedDate { get; set; }
        public string PoliceStation { get; set; }
    }
}
