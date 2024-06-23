using Microsoft.AspNetCore.Identity;

namespace ST10242546_CLDV6211_POE_.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
