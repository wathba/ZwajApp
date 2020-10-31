using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ZwajApp.Api.Helper
{
    public static  class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message){
   response.Headers.Add("Application-Error",message);
   response.Headers.Add("Access-control-Expose-headers","Application-Error");
   response.Headers.Add("Access-Control-Allow-Origin","*");
  }
  public static void AddPagination(this HttpResponse response,int currentPage, int itemsPerPage, int totalItems,int totalPages){
   var paginationHeaders = new PaginationHeaders(currentPage, itemsPerPage, totalItems, totalPages);
   var camelCaseFormatter = new JsonSerializerSettings();
  camelCaseFormatter.ContractResolver =new CamelCasePropertyNamesContractResolver();
   response.Headers.Add("pagination", JsonConvert.SerializeObject(paginationHeaders,camelCaseFormatter));
   response.Headers.Add("Access-control-Expose-headers", "pagination");

  }
       public static int CalculateAge(this DateTime dateTime) {
   var age = DateTime.Today.Year - dateTime.Year;
   if(dateTime.AddYears(age)>DateTime.Today) age--;
   return age;
  }
    }
}