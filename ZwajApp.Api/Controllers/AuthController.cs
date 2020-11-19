using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ZwajApp.Api.Data;
using ZwajApp.Api.DTOS;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Controllers
{
 [AllowAnonymous]
 [Route("api/[controller]")]
 [ApiController]

 public class AuthController : ControllerBase
 {

  private readonly IConfiguration _config;
  private readonly IMapper _mapper;
  private readonly SignInManager<User> _signInManager;
  private readonly UserManager<User> _userManager;

  public AuthController(IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
  {
   _userManager = userManager;
  _signInManager = signInManager;
   _mapper = mapper;
  
   _config = config;
  }
  [HttpPost("register")]
  public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
  {
 
   var userToCreate = _mapper.Map<User>(userForRegisterDTO);

   var result= await _userManager.CreateAsync(userToCreate, userForRegisterDTO.Password);
   var userToReturn = _mapper.Map<UserDetailsDto>(userToCreate);
   if(result.Succeeded){
 return CreatedAtRoute("GetUser", new { Controller = "user", Id = userToCreate.Id }, userToReturn);
   }
   return BadRequest("fail to Create User");




  }
  [HttpPost("login")]
  public async Task<IActionResult> Login(UserLoginDto userLoginDto)
  {
   var user = await _userManager.FindByNameAsync(userLoginDto.UserName);
   var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password,false);
   if(result.Succeeded){
    var appUser = await _userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.NormalizedUserName == userLoginDto.UserName.ToUpper());
    var userToreturn = this._mapper.Map<UserForListDto>(appUser);
     return Ok(new
   {
    token = GenerateJwtToken(appUser).Result,
    user= userToreturn 
   });
   }
   return Unauthorized();



  }
  private async Task<string> GenerateJwtToken(User user)
  {

   var claims = new List<Claim>{
     new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
     new Claim(ClaimTypes.Name,user.UserName)
  };
   var roles = await _userManager.GetRolesAsync(user);
   foreach (var role in roles)
   {
    claims.Add(new Claim(ClaimTypes.Role,role));
   }
   var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
   var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
   var tokenDescriptor = new SecurityTokenDescriptor
   {
    Subject = new ClaimsIdentity(claims),
    Expires = DateTime.Now.AddDays(1),
    SigningCredentials = creds
   };
   var tokenHandler = new JwtSecurityTokenHandler();
   var token = tokenHandler.CreateToken(tokenDescriptor);
   return tokenHandler.WriteToken(token);


  }
 }
}