using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZwajApp.Api.Data;
using ZwajApp.Api.DTOS;

namespace ZwajApp.Api.Controllers
{
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
  [HttpGet("{id}")]
  public async Task<IActionResult> GetUser(int id)
  {
   var user = await _repo.GetUser(id);
   var userReturnDetailsDto = _mapper.Map<UserDetailsDto>(user);
   return Ok(userReturnDetailsDto);
  }
 }
}