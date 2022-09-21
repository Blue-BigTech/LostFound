global using Microsoft.AspNetCore.Identity;
using NodaTime;

namespace Infrastructure
{
    public class AppUser : IdentityUser
    {
        public string DistrictCorporation { get; set; }
        public string PoliceStation { get; set; }
        public string UserPassword { get; set; }
        public Instant CreatedDate { get; set; }
    }
}
