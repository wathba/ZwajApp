using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Data
{
 public class AuthRepository : IAuthRepository
 {
  private readonly DataContext _context;
 
  public AuthRepository(DataContext context)
  {
  
   _context = context;

  }
  public async Task<User> Register(User user, string password)
  {
   byte[] passwordHash, passwordSolt;
   CreatePasswordHash(password, out passwordHash, out passwordSolt);
   user.PasswordHash = passwordHash;
   user.PasswordSalt = passwordSolt;
   await _context.Users.AddAsync(user);
   await _context.SaveChangesAsync();
   return user;
  }
  public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSolt)
  {
   using (var hmac = new System.Security.Cryptography.HMACSHA512())
   {
    passwordHash = hmac.Key;
    passwordSolt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
   }
  }

  public async Task<User> Login(string username, string password)
  {
   var user = await _context.Users.FirstOrDefaultAsync(x=>x.Name==username);
   if(user == null || !VerifyPasswordHash(password, user.PasswordSalt, user.PasswordHash)) {return null;}
   return user;
  }

  private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSolt)
  {
   using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSolt))
   {
    var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    for (int i = 0; i < ComputeHash.Length; i++)
    {
     if (ComputeHash[i] !=passwordHash[i]) {
      return false;}
    }
    return true;
   }
   
  }

  public async Task<bool> UserExists(string username)
  {
   if (await _context.Users.AnyAsync(x=>x.Name==username)) return true;
   return false;
  }
 }
}