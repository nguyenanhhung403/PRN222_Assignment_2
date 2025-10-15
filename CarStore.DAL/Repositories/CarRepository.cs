using CarStore.BO;

namespace CarStore.DAL.Repositories
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        public CarRepository(CarStoreDbContext context) : base(context)
        {
        }
    }
}
