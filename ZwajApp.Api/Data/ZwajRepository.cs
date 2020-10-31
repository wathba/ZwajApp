using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZwajApp.Api.Helper;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Data
{

 public class ZwajRepository : IZwajRepository
 {
  private readonly DataContext _context;

  public ZwajRepository(DataContext context)
  {
   _context = context;

  }
  public void Add<T>(T entity) where T : class
  {
   _context.Add(entity);
  }

  public void Delete<T>(T entity) where T : class
  {
      _context.Remove(entity);
  }

  public async Task<Photo> GetMainPhoto(int userId)
  {
   return await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
  }

  public async Task<Photo> GetPhoto(int id)
  {
   var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
   return photo;
  }

  public async Task<User> GetUser(int id)
  {
   var user = await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(x=>x.Id==id);
   return user;
  }

  public  async Task<PageList<User>> GetUsers(UserParams userParams)
  {
   var users = _context.Users.Include(u=>u.Photos).OrderByDescending(u=>u.LastActive).AsQueryable();
   users = users.Where(u => u.Id!=userParams.UserId);
   users = users.Where(u => u.Gender==userParams.Gender);
   if(userParams.MinAge!=18||userParams.MaxAge!=99){
    var minDoB = DateTime.Today.AddYears(-userParams.MaxAge - 1);
    var MaxDoB = DateTime.Today.AddYears(-userParams.MinAge);
    users = users.Where(u => u.DateOfBirth >= minDoB && u.DateOfBirth <= MaxDoB);
   }
   if(!string.IsNullOrEmpty(userParams.OrderBy)){
       switch (userParams.OrderBy)
       {
           case "created":
     users =users.OrderByDescending(u => u.Created);
      break;
     default:
      users = users.OrderByDescending(u => u.LastActive);
      break;

    }
   }
   return await PageList<User>.CreateAsync(users,userParams.PageNumber,userParams.PageSize);

  }

  
  public async Task<bool> SaveAll()
  {
   return await _context.SaveChangesAsync()>0;
  }


 }
}