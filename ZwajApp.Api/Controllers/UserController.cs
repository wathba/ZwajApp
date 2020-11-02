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
using ZwajApp.Api.Models;

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
  public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
  {
    var currentUserId=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
   var userFromRepo = await _repo.GetUser(currentUserId);
   userParams.UserId = currentUserId;
   if(string.IsNullOrEmpty(userParams.Gender)){
    userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
   }
   var users = await _repo.GetUsers(userParams);
   var usersRetunDto = _mapper.Map<IEnumerable<UserForListDto>>(users);
   Response.AddPagination(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);
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
  [HttpPost("{id}/like/{recipientId}")]
  public async Task<IActionResult> GetLike(int id,int recipientId) {
     if(id !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
   return Unauthorized();
   var like = await _repo.GetLike(id, recipientId);
   if(like!=null) return BadRequest("You Already Liked");
   if (await _repo.GetUser(recipientId)==null) return NotFound();
   like = new Like
   {
    LikerId = id,
    LikeeId = recipientId};
   _repo.Add<Like>(like);
   if(await _repo.SaveAll())
    return Ok();
   return BadRequest("Like Failed");


  }
 }
}