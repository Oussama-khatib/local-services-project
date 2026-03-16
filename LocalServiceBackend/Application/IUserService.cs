using Trial;

public interface IUserService
{
    Task<User> InsertUserAsync(User user, string plainPassword);
    Task<bool> DeleteUserAsync(int userId);
    Task<User?> UpdateUserAsync(User user);
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
    Task<bool> ActivateUserAsync(int userId);
    Task<bool> DeactivateUserAsync(int userId);
    Task<User?> LoginAsync(string email, string plainPassword);
}