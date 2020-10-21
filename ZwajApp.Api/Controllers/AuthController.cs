using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ZwajApp.Api.Data;
using ZwajApp.Api.DTOS;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class AuthController : ControllerBase
 {
  private readonly IAuthRepository _repos;
  private readonly IConfiguration _config;
  private readonly IMapper _mapper;

  public AuthController(IAuthRepository repos, IConfiguration config, IMapper mapper)
  {
   _mapper = mapper;
   _repos = repos;
   _config = config;
  }
  [HttpPost("register")]
  public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
  {
   userForRegisterDTO.Name = userForRegisterDTO.Name.ToLower();
   if (await _repos.UserExists(userForRegisterDTO.Name))
    return BadRequest("please choose other name");
   var userToCreate = _mapper.Map<User>(userForRegisterDTO);

   var userCreate = await _repos.Register(userToCreate, userForRegisterDTO.Password);
   var userToReturn = _mapper.Map<UserDetailsDto>(userCreate);

   return CreatedAtRoute("GetUser",new {Controller="user",Id=userCreate.Id},userToReturn);

  }
  [HttpPost("login")]
  public async Task<IActionResult> Login(UserLoginDto userLoginDto)
  {
   var userFromRepo = await _repos.Login(userLoginDto.Username.ToLower(), userLoginDto.Password);
   if (userFromRepo == null) return Unauthorized();
   var claims = new[]{
     new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
     new Claim(ClaimTypes.Name,userFromRepo.Name)
  };
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
   var user = this._mapper.Map<UserForListDto>(userFromRepo);
   return Ok(new
   {
    token = tokenHandler.WriteToken(token),
    user
   });

  }
 }
}