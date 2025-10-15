using CarStore.BO;
using CarStore.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.BLL.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _carRepository.GetByIdAsync(id);
        }

        public async Task CreateCarAsync(Car car)
        {
            await _carRepository.InsertAsync(car);
            await _carRepository.SaveAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            _carRepository.Update(car);
            await _carRepository.SaveAsync();
        }

        public async Task DeleteCarAsync(int id)
        {
            await _carRepository.DeleteAsync(id);
            await _carRepository.SaveAsync();
        }
    }
}
