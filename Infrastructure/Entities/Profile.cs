using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure
{
    public class Profile : BaseEntity
    {
        public string Name { get; set; }
        public string CPName { get; set; }
        public string CPPhoneNo { get; set; }
        public string GDCaseNo { get; set; }
        public string OfficerName { get; set; }
        public string OfficerBPNo { get; set; }
        public string OfficerPhoneNo { get; set; }
        public string Remarks { get; set; }
        public string ImageURL { get; set; }

        public bool IsLost { get; set; }
        public Instant CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Instant UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        [ForeignKey("AppUser")]
        public string AspNetUserId { get; set; }
        public virtual AppUser AspNetUser { get; set; }
    }
}
