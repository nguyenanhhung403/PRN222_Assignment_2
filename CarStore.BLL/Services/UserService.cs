using CarStore.BO;
using CarStore.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CarStore.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var users = await _userRepository.GetAllAsync();
            // Note: Password should be hashed in a real application
            return users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        }

        public async Task RegisterAsync(User user)
        {
            // Note: Add validation and password hashing here
            await _userRepository.InsertAsync(user);
            await _userRepository.SaveAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
    }
}
