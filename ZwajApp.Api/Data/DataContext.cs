using Microsoft.EntityFrameworkCore;
using ZwajApp.Api.Models;

namespace ZwajApp.Api.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> Options):base(Options)
        {
        
        }
        
        public DbSet<Value> Values { get; set; }
    }
}