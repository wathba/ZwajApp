using Microsoft.AspNetCore.Identity;
using ZwajApp.Api.Model;

namespace ZwajApp.Api.Models
{
    public class UserRole:IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}