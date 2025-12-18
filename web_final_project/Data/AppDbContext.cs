using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Models;

namespace OnlineBookStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Reviews> Reviews => Set<Reviews>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed admin user and some categories
            var admin = new AppUser { Id = 1, Email = "admin@gmail.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), Role = "Admin" };

            modelBuilder.Entity<AppUser>().HasData(admin);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "IT" },
                new Category { Id = 2, Name = "History" },
                new Category { Id = 3, Name = "Classics" }
            );
        }
    }
}












