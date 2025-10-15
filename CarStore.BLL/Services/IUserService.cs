using CarStore.BO;
using System.Threading.Tasks;

namespace CarStore.BLL.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task RegisterAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
