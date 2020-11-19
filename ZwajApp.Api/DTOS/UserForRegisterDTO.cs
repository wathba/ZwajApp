using System;
using System.ComponentModel.DataAnnotations;

namespace ZwajApp.Api.DTOS
{
    public class UserForRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [StringLength(8,MinimumLength=4,ErrorMessage="this account has been registerd")]
         [Required]
         public string Password{ get; set; }
         [Required]
        public string  Gender { get; set; }
         [Required]
        public DateTime DateOfBirth { get; set; }
         [Required]
        public string City { get; set; }
         [Required]
        public string Country{ get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public UserForRegisterDTO()
        {
   Created = DateTime.Now;
   LastActive = DateTime.Now;
  }
    }
    
}