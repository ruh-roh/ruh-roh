using Microsoft.EntityFrameworkCore;

namespace RuhRoh.Samples.WebAPI.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
            
        }
    }
}