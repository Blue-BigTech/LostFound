
using Common.Abstract.Extension;
using Common.Abstract.Helpers;
using Model;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
namespace Common.Dto
{
    public class LoginModel
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
        public string AspNetUserId { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public byte[] Image { get; set; }
    }
    public class UserModel
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string DistrictCorporation { get; set; }
        public string PoliceStation { get; set; }
    }
    public class ResetViewModel
    {
        public string Code { get; set; }
        public string UserPassword { get; set; }


    }
    public class Pagination
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string SortOrder { get; set; }
        public int SortCol { get; set; }
        public string AspNetRoleId { get; set; }
        public string AspNetUserId { get; set; }

    }
    public class AllPhotosResponseModel
    {
        public long Total { get; set; }
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        [NotMapped]
        public string Code { get { return Id.Encode(); } }
        public Int64 RowNo { get; set; }
        public string Name { get; set; }
        public string CPName { get; set; }
        public string CPPhoneNo { get; set; }
        public string GDCaseNo { get; set; }
        public string OfficerName { get; set; }
        public string OfficerBPNo { get; set; }
        public string OfficerPhoneNo { get; set; }
        public string Remarks { get; set; }
        [JsonIgnore]
        public string ImageURL { get; set; }      
        [NotMapped, System.Text.Json.Serialization.JsonPropertyName("Image")]
        public string ImageUrl
        {
            get
            {
                return string.IsNullOrEmpty(ImageURL) ? Settings.ImageUrl + "defaultImage.png" : Settings.ImageUrl + ImageURL;
            }
        }
        public string IsLost { get; set; }
        public string CreatedDate { get; set; }
        public string PoliceStation { get; set; }
    }
}
