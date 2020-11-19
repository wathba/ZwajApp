using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Model
{
    public class Role:IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}