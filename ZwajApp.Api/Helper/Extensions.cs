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
        
    }
}