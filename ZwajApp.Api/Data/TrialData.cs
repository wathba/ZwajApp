using System.Collections.Generic;
using Newtonsoft.Json;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Data
{
 public class TrialData
 {
  private readonly DataContext _context;
  public TrialData(DataContext context)
  {
   _context = context;
  }
  public void TrialUsers(){
   var userData = System.IO.File.ReadAllText("Data/UserTrialData.json");
   var users = JsonConvert.DeserializeObject<List<User>>(userData);
   foreach (var user in users)
   {
    byte[] passwordHash, passwordSold;
    CreatePasswordHash("password", out passwordHash, out passwordSold);
    user.PasswordHash = passwordHash;
    user.PasswordSalt = passwordSold;
    user.Name = user.Name.ToLower();
    _context.Add(user);
   }
   _context.SaveChanges();
  }
  public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSolt)
  {
   using (var hmac = new System.Security.Cryptography.HMACSHA512())
   {
    passwordHash = hmac.Key;
    passwordSolt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
   }
  }
 }
}