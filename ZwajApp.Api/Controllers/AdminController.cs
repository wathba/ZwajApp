using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZwajApp.Api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZwajApp.Api.DTOS;
using Microsoft.AspNetCore.Identity;
using ZwajApp.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace ZwajApp.Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class AdminController : ControllerBase
 {
  private readonly DataContext _context;


  private readonly UserManager<User> _userManager;

  public AdminController(DataContext context, UserManager<User> userManager)
  {
   _userManager = userManager;
   _context = context;

  }

  [HttpGet("userWithRoles")]
  public async Task<IActionResult> GetUsersWithRoles()
  {
   var userList = await (from user in _context.Users
                         orderby user.UserName
                         select new
                         {
                          Id = user.Id,
                          UserName = user.UserName,
                          Roles = (from userRole in user.UserRoles
                                   join role in _context.Roles
                                   on userRole.RoleId
                                   equals role.Id
                                   select role.Name).ToList()
                         }).ToListAsync();
   return Ok(userList);
  }




  [HttpGet("photoForModeration")]
  public IActionResult GetPhotoForModerator()
  {
   return Ok("Allow ForModerator and  Admin Only");
  }
  [Authorize(Roles="Admin")]
 [HttpPost("EditRoles/{userName}")]
 public async Task<IActionResult> EditUserRoles(string userName, EditRolesDto editRolesDto){
   var user = await _userManager.FindByNameAsync(userName);
   var userRoles = await _userManager.GetRolesAsync(user);
   var selectedRoles = editRolesDto.RoleNames;
   selectedRoles = selectedRoles?? new string[] { };
   var result = await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles));
   if(!result.Succeeded)
    return BadRequest("There is a mistake for adding Roles");
   result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
    if(!result.Succeeded)
    return BadRequest("There is a mistake for Removing Roles");
   return Ok(await _userManager.GetRolesAsync(user));
  }

 }

}