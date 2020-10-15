using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZwajApp.Api.Data;
using ZwajApp.Api.Helper;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using ZwajApp.Api.DTOS;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using ZwajApp.Api.Models;
using System.Linq;

namespace ZwajApp.Api.Controllers
{
 [Authorize]
 [Route("api/user/{userId}/photos")]
 [ApiController]
 public class PhotosController : ControllerBase
 {
  private readonly IZwajRepository _repo;

  private readonly IMapper _mapper;
  private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
  private Cloudinary _cloudinary;

  public PhotosController(IZwajRepository repo, IOptions<CloudinarySettings> cloudinaryConfig, IMapper mapper)
  {
   _cloudinaryConfig = cloudinaryConfig;
   _mapper = mapper;

   _repo = repo;
   Account account = new Account(
          _cloudinaryConfig.Value.CloudName,
         _cloudinaryConfig.Value.ApiKey,
         _cloudinaryConfig.Value.ApiSecret
        );

   _cloudinary = new Cloudinary(account);
  }
  [HttpGet("{id}", Name = "GetPhoto")]
  public async Task<IActionResult> GetPhoto(int id)
  {
   var photoFromRepository = await _repo.GetPhoto(id);
   var photo = _mapper.Map<PhotoForReturedDto>(photoFromRepository);
   return Ok(photo);
  }
  [HttpPost]
  public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] PhotoForCreateDto photoForCreateDto)
  {

   if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var userFromRepo = await _repo.GetUser(userId);
   var file = photoForCreateDto.File;
   var uploadResult = new ImageUploadResult();
   if (file != null && file.Length > 0)
   {
    using (var stream = file.OpenReadStream())
    {
     var imageUploadParams = new ImageUploadParams()
     {
      File = new FileDescription(file.Name, stream),
      Transformation = new Transformation()
      .Width(500).Height(500).Crop("fill").Gravity("face")
     };

     uploadResult = _cloudinary.Upload(imageUploadParams);
    }
   }
   photoForCreateDto.Url = uploadResult.Uri.ToString();
   photoForCreateDto.PublicId = uploadResult.PublicId;
   var photo = _mapper.Map<Photo>(photoForCreateDto);
   if (!userFromRepo.Photos.Any(p => p.IsMain))
    photo.IsMain = true;
   userFromRepo.Photos.Add(photo);
   if (await _repo.SaveAll())
   {
    var photoToReturn = _mapper.Map<PhotoForReturedDto>(photo);
    return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
   }

   return BadRequest("there is problem in the image");
  }
  [HttpPost("{id}/setMain")]
  public async Task<IActionResult> SetMainPohto(int userId, int id)
  {
   if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var userFromRepo = await _repo.GetUser(userId);
   if (!userFromRepo.Photos.Any(p => p.Id == id))
    return Unauthorized();
   var desiredMainPhoto = await _repo.GetPhoto(id);
   if (desiredMainPhoto.IsMain)
    return BadRequest("this is the main photo");
   var currentMainPhoto = await _repo.GetMainPhoto(userId);
   currentMainPhoto.IsMain = false;
   desiredMainPhoto.IsMain = true;
   if (await _repo.SaveAll())
    return NoContent();
   return BadRequest("There is a problem ");


  }
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeletePhoto(int id, int userId)
  {
   if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
    return Unauthorized();
   var userFromRepo = await _repo.GetUser(userId);
   if (!userFromRepo.Photos.Any(p => p.Id == id))
    return Unauthorized();
   var photo = await _repo.GetPhoto(id);
   if (photo.IsMain)
    return BadRequest("this is the main photo");
   if (photo.PublicId != null)
   {
    var deleteParams = new DeletionParams(photo.PublicId);
    var result = this._cloudinary.Destroy(deleteParams);
    if (result.Result == "ok")
    {
     _repo.Delete(photo);
    }
   }
   if (photo.PublicId == null)
   {
    _repo.Delete(photo);
   }
   if (await _repo.SaveAll())
    return Ok();
   return BadRequest("Fail To Delete the photo");

  }

 }
}