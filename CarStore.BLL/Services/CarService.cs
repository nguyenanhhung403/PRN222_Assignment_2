using CarStore.BO;
using CarStore.DAL.Repositories;
using Microsoft.AspNetCore.SignalR;
using CarStore.WebUI.Hubs;
using CarStore.WebUI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.BLL.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IHubContext<InventoryHub> _inventoryHubContext;

        public CarService(ICarRepository carRepository, IHubContext<InventoryHub> inventoryHubContext)
        {
            _carRepository = carRepository;
            _inventoryHubContext = inventoryHubContext;
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
            // Get the previous stock value before updating
            var existingCar = await _carRepository.GetByIdAsync(car.CarId);
            var previousStock = existingCar?.Stock ?? 0;

            _carRepository.Update(car);
            await _carRepository.SaveAsync();

            // Send SignalR notification if stock changed
            if (previousStock != car.Stock)
            {
                var stockChange = (car.Stock ?? 0) - previousStock;
                var updateType = stockChange > 0 ? "Restock" : "Sale";

                var notification = new InventoryUpdateDto
                {
                    CarId = car.CarId,
                    Brand = car.Brand,
                    Model = car.Model,
                    PreviousStock = previousStock,
                    CurrentStock = car.Stock ?? 0,
                    StockChange = stockChange,
                    UpdateType = updateType,
                    UpdatedAt = DateTime.Now,
                    Message = $"{car.Brand} {car.Model} stock updated: {previousStock} → {car.Stock}"
                };

                // Broadcast to all clients watching inventory
                await _inventoryHubContext.Clients.All.SendAsync("ReceiveInventoryUpdate", notification);

                // Also notify specific car watchers
                await _inventoryHubContext.Clients.Group($"car_{car.CarId}").SendAsync("ReceiveCarStockUpdate", notification);
            }
        }

        public async Task DeleteCarAsync(int id)
        {
            await _carRepository.DeleteAsync(id);
            await _carRepository.SaveAsync();
        }
    }
}
