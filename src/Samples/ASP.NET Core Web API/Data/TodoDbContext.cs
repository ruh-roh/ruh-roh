using Microsoft.EntityFrameworkCore;
using RuhRoh.Samples.WebAPI.Domain;

namespace RuhRoh.Samples.WebAPI.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .Property(x => x.Description)
                .HasMaxLength(200)
                .IsUnicode()
                .IsRequired();
        }
    }
}