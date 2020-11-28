using System;
using System.Collections.Generic;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.DTOS
{
    public class UserDetailsDto
    {
            public int Id { get; set; }
       public string UserName { get; set; }
       public int Age{ get; set; }
       public string Gender { get; set; }
       public string KownAs{ get; set; }
       public string Created { get; set; }
       public DateTime LastActive { get; set; }
       public string Introduction { get; set; }
       public string LookingFor { get; set; }
       public string Interests { get; set; }
       public string City { get; set; }
       public string Country { get; set; }
       public string PhotoUrl { get; set; }
       public ICollection<PhotoForDetailsDto> Photos { get; set; }
    }
}