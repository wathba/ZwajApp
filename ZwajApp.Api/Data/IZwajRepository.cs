using System.Collections.Generic;
using System.Threading.Tasks;
using ZwajApp.Api.Helper;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Data
{
 public interface IZwajRepository
 {
  void Add<T>(T entity) where T : class;
  void Delete<T>(T entity) where T : class;
   Task<bool> SaveAll();
  Task<PageList<User>> GetUsers(UserParams userParams);
  Task<User> GetUser(int id);
  Task<Photo> GetPhoto(int id);
  Task<Photo> GetMainPhoto(int userId);
  Task<Like> GetLike(int userId,int recipientId);
 }
}