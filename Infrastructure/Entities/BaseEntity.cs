using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    public class BaseEntity
    {
        [Key, Required]
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }
}
