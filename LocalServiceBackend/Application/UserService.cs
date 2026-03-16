using Infrastructure;
using Trial;

namespace Application
{
    public class UserService : IUserService
    {

        private byte[] HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }


        public async Task<User> InsertUserAsync(User user, string plainPassword)
        {
            UserRepository userRepo = new UserRepository();
            var existingUser = await userRepo.GetByEmailAsync(user.Email);
            if (existingUser != null)
                throw new Exception("Email already exists");

            if (user.Role!="Admin"&&user.Role!="Customer"&&user.Role!="Service Provider")
                throw new Exception("Role is not Correct");

            user.Password = HashPassword(plainPassword);
            user.CreatedAt = DateTime.Now;
            user.IsActive = "yes";
            await userRepo.InsertUserAsync(user);
            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            UserRepository userRepo = new UserRepository();
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            await userRepo.DeleteUserAsync(userId);
            return true;
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            UserRepository userRepo = new UserRepository();
            var existingUser = await userRepo.GetByIdAsync(user.UserId);
            if (existingUser == null) return null;
            existingUser.FullName = user.FullName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Role = user.Role;
            await userRepo.UpdateUserAsync(existingUser);
            return existingUser;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            UserRepository userRepo = new UserRepository();
            return await userRepo.GetByIdAsync(userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            UserRepository userRepo = new UserRepository();
            return await userRepo.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            UserRepository userRepo = new UserRepository();
            return await userRepo.GetAllAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            UserRepository userRepo = new UserRepository();
            return await userRepo.GetByRoleAsync(role);
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            UserRepository userRepo = new UserRepository();
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null) return false;
            await userRepo.ActivateAsync(userId);
            return true;
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            UserRepository userRepo = new UserRepository();
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            await userRepo.DeactivateAsync(userId);
            return true;
        }

        public async Task<User?> LoginAsync(string email, string plainPassword)
        {
            UserRepository userRepo = new UserRepository();
            var hashedPassword = HashPassword(plainPassword);
            var user = await userRepo.ValidateCredentialsAsync(email, hashedPassword);
            if (user == null || user.IsActive=="no")
                return null;
            return user;
        }



    }
}
