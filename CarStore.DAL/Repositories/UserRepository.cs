using CarStore.BO;

namespace CarStore.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CarStoreDbContext context) : base(context)
        {
        }
    }
}
