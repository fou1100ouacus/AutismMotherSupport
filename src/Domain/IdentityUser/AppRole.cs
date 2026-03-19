using Microsoft.AspNetCore.Identity;
namespace Domain.Models.IdentityUser

{
    public class AppRole : IdentityRole<int>
    {
        public string? Description { get; set; }
    }
}
