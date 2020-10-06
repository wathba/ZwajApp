using System;
using Microsoft.AspNetCore.Http;

namespace ZwajApp.Api.Helper
{
    public static  class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message){
   response.Headers.Add("Application-Error",message);
   response.Headers.Add("Access-control-Expose-headers","Application-Error");
   response.Headers.Add("Access-Control-Allow-Origin","*");
  }
       public static int CalculateAge(this DateTime dateTime) {
   var age = DateTime.Today.Year - dateTime.Year;
   if(dateTime.AddYears(age)>DateTime.Today) age--;
   return age;
  }
    }
}