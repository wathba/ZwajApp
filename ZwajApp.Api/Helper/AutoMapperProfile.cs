using System.Linq;
using AutoMapper;
using ZwajApp.Api.DTOS;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Helper
{
    public class AutoMapperProfile:Profile
    {
       public AutoMapperProfile()
       {
   CreateMap<User, UserForListDto>()
   .ForMember(dest => dest.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); })
   .ForMember(dest => dest.Age, opt => { opt.ResolveUsing(src => src.DateOfBirth.CalculateAge()); });

   CreateMap<User, UserDetailsDto>()
   .ForMember(dest => dest.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); })
   .ForMember(dest => dest.Age, opt => { opt.ResolveUsing(src => src.DateOfBirth.CalculateAge()); });
  CreateMap<Photo,PhotoForDetailsDto>();
   CreateMap<UserForUpdateDto, User>();
   CreateMap<Photo,PhotoForReturedDto>();
   CreateMap<PhotoForCreateDto,Photo>();
   CreateMap<UserForRegisterDTO,User>();
   CreateMap<MessageForCeationDto,Message>().ReverseMap();
   CreateMap<Message, MessageToReturnDto>()
   .ForMember(dest => dest.SenderPhotoUrl, opt => { opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain).Url); })
   .ForMember(dest => dest.RecipientPhotoUrl, opt => { opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url); });

  }
    }
}