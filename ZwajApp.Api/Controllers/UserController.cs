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
using Microsoft.Extensions.Options;
using Stripe;
using DinkToPdf;
using System.IO;
using DinkToPdf.Contracts;

namespace ZwajApp.Api.Controllers
{
 [ServiceFilter(typeof(LogUserActivity))]

 [Route("api/[controller]")]
 [ApiController]
 public class UserController : ControllerBase
 {
  private readonly IZwajRepository _repo;
  private readonly IMapper _mapper;
  private readonly IOptions<StripeSettings> _stripeSettings;
  private readonly IConverter _converter;
  public UserController(IZwajRepository repo, IMapper mapper, IOptions<StripeSettings> stripeSettings, IConverter converter)
  {
   _converter = converter;
   _stripeSettings = stripeSettings;
   _mapper = mapper;
   _repo = repo;
  }
  [HttpGet]
  public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
  {
   var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

   var userFromRepo = await _repo.GetUser(currentUserId);
   userParams.UserId = currentUserId;
   if (string.IsNullOrEmpty(userParams.Gender))
   {
    userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
   }
   var users = await _repo.GetUsers(userParams);
   var usersRetunDto = _mapper.Map<IEnumerable<UserForListDto>>(users);
   Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
   return Ok(usersRetunDto);
  }
  [HttpGet("{id}", Name = "GetUser")]
  public async Task<IActionResult> GetUser(int id)
  {
   var user = await _repo.GetUser(id);
   var userReturnDetailsDto = _mapper.Map<UserDetailsDto>(user);
   return Ok(userReturnDetailsDto);
  }
  [HttpPut("{id}")]
  public async Task<IActionResult> UserForUpdate(int id, UserForUpdateDto userForUpdateDto)
  {
   if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var userFromRepo = await _repo.GetUser(id);
   _mapper.Map(userForUpdateDto, userFromRepo);
   if (await _repo.SaveAll())
    return NoContent();

   throw new System.Exception($"there is problem for user with {id}");
  }
  [HttpPost("{id}/like/{recipientId}")]
  public async Task<IActionResult> GetLike(int id, int recipientId)
  {
   if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var like = await _repo.GetLike(id, recipientId);
   if (like != null) return BadRequest("You Already Liked");
   if (await _repo.GetUser(recipientId) == null) return NotFound();
   like = new Like
   {
    LikerId = id,
    LikeeId = recipientId
   };
   _repo.Add<Like>(like);
   if (await _repo.SaveAll())
    return Ok();
   return BadRequest("Like Failed");


  }
  [HttpPost("{userId}/charge/{stripeToken}")]
  public async Task<IActionResult> Charge(int userId, string stripeToken)
  {
   if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var customers = new CustomerService();
   var charges = new ChargeService();
   var customer = customers.Create(new CustomerCreateOptions
   {
    Source = stripeToken
   });
   var charge = charges.Create(new ChargeCreateOptions
   {
    Amount = 5000,
    Description = "App SubsCription",
    Currency = "usd",
    Customer = customer.Id
   });
   var payment = new Payment
   {
    PaymentDate = DateTime.Now,
    Amount = charge.Amount / 100,
    UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
    ReceiptUrl = charge.ReceiptUrl,
    Description = charge.Description,
    Currency = charge.Currency,
    IsPaid = charge.Paid

   };
   _repo.Add<Payment>(payment);
   await _repo.SaveAll();
   { return Ok(new { IsPaid = charge.Paid }); }




  }
  [HttpGet("userId/charge")]
  public async Task<IActionResult> GetPaymentsForUser(int userId)
  {
   if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var payment = await _repo.GetPaymentForUser(userId);
   return Ok(payment);

  }

  [Authorize(Policy = "RequireAdimRole")]
  [HttpGet("UserReport/{userId}")]
  public IActionResult CreatePdfForUser(int userId)
  {
   var templateGenerator = new TemplateGenerator(_repo, _mapper);
   var globalSettings = new GlobalSettings
   {
    ColorMode = ColorMode.Color,
    Orientation = Orientation.Portrait,
    PaperSize = PaperKind.A4,
    Margins = new MarginSettings { Top = 15, Bottom = 20 },
    DocumentTitle = "Member Card"

   };

   var objectSettings = new ObjectSettings
   {
    PagesCount = true,
    HtmlContent = templateGenerator.GetHTMLStringForUser(userId),
    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
    HeaderSettings = { FontName = "Impact", FontSize = 12, Spacing = 5, Line = false },
    FooterSettings = { FontName = "Geneva", FontSize = 15, Spacing = 7, Line = true, Center = "ZwajApp By Eng Muhammad  Jabeer", Right = "[page]" }
   };

   var pdf = new HtmlToPdfDocument()
   {
    GlobalSettings = globalSettings,
    Objects = { objectSettings }
   };

   var file = _converter.Convert(pdf);
   return File(file, "application/pdf");
  }
  [Authorize(Policy = "RequireAdimRole")]
  [HttpGet("GetAllUsersExceptAdmin")]
  public async Task<IActionResult> GetAllUsersExceptAdmin(){
   var users = await _repo.GetAllUsersEceptAdmin();
   var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
   return Ok(usersToReturn);
  }

 }
}