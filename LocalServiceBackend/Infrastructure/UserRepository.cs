
using Microsoft.EntityFrameworkCore;
using Trial;

namespace Infrastructure
{
    public class UserRepository
    {
        public async Task InsertUserAsync(User user)
        {
            using (var context = new AppDBContext())
            {
                var newUser = new User
                {
                    FullName =user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Password = user.Password,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedAt =DateTime.Now,
                };
                await context.Users.AddAsync(newUser);
                context.SaveChanges();
            }
        }

        public async Task DeleteUserAsync(int UserId)
        {
            using (var context = new AppDBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == UserId);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateUserAsync(User User)
        {
            using (var context = new AppDBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == User.UserId);
                if (user != null)
                {
                    user.FullName = User.FullName;
                    user.Email = User.Email;
                    user.PhoneNumber = User.PhoneNumber;
                    user.Password = User.Password;
                    user.Role = User.Role;
                    user.IsActive=User.IsActive;
                    context.SaveChanges();
                }
            }
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            using (var context = new AppDBContext())
            {
                return await context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using (var context = new AppDBContext())
            {
                return await context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (var context = new AppDBContext())
            {
                return await context.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(string role)
        {
            using (var context = new AppDBContext())
            {
                return await context.Users
                    .Where(u => u.Role == role)
                    .ToListAsync();
            }
        }

        public async Task ActivateAsync(int userId)
        {
            using (var context = new AppDBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);

                user.IsActive = "yes";
                await context.SaveChangesAsync();
            }
        }

        public async Task DeactivateAsync(int userId)
        {
            using (var context = new AppDBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);
                user.IsActive = "no";
                await context.SaveChangesAsync();
            }
        }

        public async Task<User?> ValidateCredentialsAsync(string email, byte[] hashedPassword)
        {
            using (var context = new AppDBContext())
            {
                return await context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == email &&
                u.Password == hashedPassword);
            }
        }

    }
}
