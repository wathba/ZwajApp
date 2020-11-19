using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ZwajApp.Api.Model;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Data
{
 public class TrialData
 {
  private readonly UserManager<User> _userManager;
  private readonly RoleManager<Role> _roleManager;

  public TrialData(UserManager<User> userManager, RoleManager<Role> roleManager)
  {
   _roleManager = roleManager;
   _userManager = userManager;

  }
  public void TrialUsers()
  {
   if (!_userManager.Users.Any())
   {
    var userData = System.IO.File.ReadAllText("Data/UserTrialData.json");
    var users = JsonConvert.DeserializeObject<List<User>>(userData);
    var roles = new List<Role>
    {
     new Role{Name="Admin"},
     new Role{Name="Moderator"},
     new Role{Name="Member"},
     new Role{Name="VIP"}
   };
   foreach ( var role in roles)
   {
     _roleManager.CreateAsync(role).Wait();
    }
    foreach (var user in users)
    {
     _userManager.CreateAsync(user, "password").Wait();
     _userManager.AddToRoleAsync(user, "Member").Wait();

    }
    var adminUser = new User
    {
     UserName = "Admin"
   };
    IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;
    var admin = _userManager.FindByNameAsync("Admin").Result;
    _userManager.AddToRolesAsync(admin,new[]{ "Admin", "Moderator" }).Wait();
   }


  }

 }
}