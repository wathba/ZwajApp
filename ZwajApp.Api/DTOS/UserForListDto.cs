using System;

namespace ZwajApp.Api.DTOS
{
    public class UserForListDto
    {
            public int Id { get; set; }
       public string UserName { get; set; }
       public int Age { get; set; }
       public string Gender { get; set; }
       public string KownAs{ get; set; }
       public DateTime Created { get; set; }
       public DateTime LastActive { get; set; }
       public string City { get; set; }
       public string Country { get; set; }
       public string PhotoUrl { get; set; }
      
    }
}