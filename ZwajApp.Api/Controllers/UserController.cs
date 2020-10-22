using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZwajApp.Api.Data;
using ZwajApp.Api.DTOS;
using System;
using ZwajApp.Api.Helper;

namespace ZwajApp.Api.Controllers
{
   [ServiceFilter(typeof(LogUserActivity))]
 [Authorize]
 [Route("api/[controller]")]
 [ApiController]
 public class UserController : ControllerBase
 {
  private readonly IZwajRepository _repo;
  private readonly IMapper _mapper;
  public UserController(IZwajRepository repo, IMapper mapper)
  {
   _mapper = mapper;
   _repo = repo;
  }
  [HttpGet]
  public async Task<IActionResult> GetUsers()
  {
   var users = await _repo.GetUsers();
   var usersRetunDto = _mapper.Map<IEnumerable<UserForListDto>>(users);
   return Ok(usersRetunDto);
  }
  [HttpGet("{id}",Name="GetUser")]
  public async Task<IActionResult> GetUser(int id)
  {
   var user = await _repo.GetUser(id);
   var userReturnDetailsDto = _mapper.Map<UserDetailsDto>(user);
   return Ok(userReturnDetailsDto);
  }
  [HttpPut("{id}")]
  public async Task<IActionResult> UserForUpdate(int id,UserForUpdateDto userForUpdateDto)
  {
  if(id !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
   return Unauthorized();
   var userFromRepo = await _repo.GetUser(id);
   _mapper.Map(userForUpdateDto,userFromRepo);
   if(await  _repo.SaveAll())
    return NoContent();
   
    throw new System.Exception($"there is problem for user with {id}");
  }
 }
}