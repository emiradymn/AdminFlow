using Microsoft.AspNetCore.Identity;

namespace AdminFlow.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }

    }
}