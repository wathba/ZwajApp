using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZwajApp.Api.Data;
using ZwajApp.Api.DTOS;
using ZwajApp.Api.Helper;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Controllers
{
 [ServiceFilter(typeof(LogUserActivity))]
 [Authorize]
 [Route("api/user/{userId}/[controller]")]
 [ApiController]
 public class MessagesController : ControllerBase
 {
  private readonly IZwajRepository _repo;
  private readonly IMapper _mapper;
  public MessagesController(IZwajRepository repo, IMapper mapper)
  {
   _mapper = mapper;
   _repo = repo;

  }
  [HttpGet("{id}",Name="GetMessage")]
  public async Task<IActionResult> GetMessage(int id,int userId){
      if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var messageFromRepo = await _repo.GetMessage(id);
   if(messageFromRepo==null)
    return NotFound();
   return Ok(messageFromRepo);
  }
  [HttpGet]
  public async Task<IActionResult> GetMessagesForUser(int userId,[FromQuery]MessagesParams messagesParams){
    if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   messagesParams.UserId = userId;
   var messagesfromRepo = await _repo.GetMessagesForUser(messagesParams);
   var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesfromRepo);
   Response.AddPagination(messagesfromRepo.CurrentPage, messagesfromRepo.TotalPages, messagesfromRepo.TotalCount, messagesfromRepo.PageSize);
   return Ok(messages);
  }
  [HttpPost()]
  public async Task<IActionResult> CreateMessage(int userId,MessageForCeationDto messageForCreationDto){
   var sender = await _repo.GetUser(userId);
   if(sender.Id!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   messageForCreationDto.SenderId = userId;
   var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);
   if(recipient==null)
    return BadRequest("Recipient Not Found");
   var message = _mapper.Map<Message>(messageForCreationDto);
   
   _repo.Add(message);
   if(await _repo.SaveAll()){
     var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
      return CreatedAtRoute("GetMessage", new { id = message.Id }, messageToReturn);
}
   throw new Exception("there is a problem To Save this Message");
  }
  [HttpGet("chat/{recipientId}")]
public async Task<IActionResult> GetConversation(int userId,int recipientId){
       if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var messagesFromRepo = await _repo.GetConversation(userId, recipientId);
   var messagesToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
   return Ok(messagesToReturn);

  }
   [HttpGet("count")]
 public async Task<IActionResult> GetUnreadMessagesForUser(int userId){
   if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
    var count = await _repo.GetUnreadMessagesForUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
   return Ok(count);

  }
  [HttpPost("read/{id}")]
  public async Task<IActionResult> MarkMessageAsRead(int userId,int id){
    if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var message =  await _repo.GetMessage(id);
   if(message.RecipientId !=userId) return Unauthorized();
   message.IsRead = true;
   message.DateRead = DateTime.Now;
   await _repo.SaveAll();
   return NoContent();
  }
  [HttpPost("{id}")]
  public async Task<IActionResult> DeleteMessage(int id,int userId){
   if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var message =  await _repo.GetMessage(id);
   if(message.SenderId==userId)
    message.SenderDelelted = true;
    if(message.RecipientId==userId)
    message.RecipientDelelted = true;
    if(message.SenderDelelted&&message.RecipientDelelted)
    _repo.Delete(message);
    if(await _repo.SaveAll())
    return NoContent();
   throw new Exception(" there is problem to delete message");
  }
 }

}