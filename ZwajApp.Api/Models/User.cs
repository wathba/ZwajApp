using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ZwajApp.Api.Models
{
    public class User:IdentityUser<int>
    {
     
     
       public DateTime DateOfBirth { get; set; }
       public string Gender { get; set; }
       public string KownAs{ get; set; }
       public DateTime Created { get; set; }
       public DateTime LastActive { get; set; }
       public string Introduction { get; set; }
       public string LookingFor { get; set; }
       public string Interests { get; set; }
       public string City { get; set; }
       public string Country { get; set; }
       public ICollection<Photo> Photos { get; set; }
       public ICollection<Like> Likers{ get; set; }
       public ICollection<Like> Likees{ get; set; }
       public ICollection<Message> MessagesSent{ get; set; }
       public ICollection<Message> MessagesRecieved{ get; set; }
       public ICollection<UserRole> UserRoles { get; set; }
    }
}