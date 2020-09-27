using System.ComponentModel.DataAnnotations;

namespace ZwajApp.Api.DTOS
{
    public class UserForRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [StringLength(8,MinimumLength=4,ErrorMessage="this account has been registerd")]
        public string Password{ get; set; }
    }
}