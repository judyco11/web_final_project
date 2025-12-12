using Microsoft.AspNetCore.Identity;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher<User> _hasher;

        public AuthService(AppDbContext db)
        {
            _db = db;
            _hasher = new PasswordHasher<User>();
        }

        public User? ValidateUser(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return null;
            var res = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return res == PasswordVerificationResult.Success ? user : null;
        }

        public User CreateUser(string username, string password, string? phone = null, string? address = null, Role role = Role.Customer)
        {
            var user = new User
            {
                Username = username,
                Phone = phone,
                Address = address,
                Role = role
            };
            user.PasswordHash = _hasher.HashPassword(user, password);
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }
    }
}
