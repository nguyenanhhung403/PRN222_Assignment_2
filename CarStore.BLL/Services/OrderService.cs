using CarStore.BO;
using CarStore.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task CreateOrderAsync(Order order)
        {
            // Add business logic for creating an order, e.g., validation, calculation
            await _orderRepository.InsertAsync(order);
            await _orderRepository.SaveAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _orderRepository.Update(order);
            await _orderRepository.SaveAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }
    }
}
