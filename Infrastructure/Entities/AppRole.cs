using NodaTime;
namespace Infrastructure
{
    public class AppRole : IdentityRole
    {
        public bool IsActive { get; set; } = true;
        public Instant CreatedDate { get; set; }
        public bool IsSystem { set; get; }
    }
}
