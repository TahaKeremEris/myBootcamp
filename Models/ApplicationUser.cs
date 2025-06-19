using BootcampDay1.Models;

using Microsoft.AspNetCore.Identity;

namespace BootcampDay1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Job { get; set; }
        public string? ProfileImagePath { get; set; }
    }
}
