using CarStore.BO;
using CarStore.DAL.Repositories;
using Microsoft.AspNetCore.SignalR;
using CarStore.WebUI.Hubs;
using CarStore.WebUI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHubContext<OrderHub> _orderHubContext;
        private readonly IHubContext<AdminHub> _adminHubContext;

        public OrderService(
            IOrderRepository orderRepository,
            IHubContext<OrderHub> orderHubContext,
            IHubContext<AdminHub> adminHubContext)
        {
            _orderRepository = orderRepository;
            _orderHubContext = orderHubContext;
            _adminHubContext = adminHubContext;
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

            // Send SignalR notification to admin group
            var notification = new OrderNotificationDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserName = order.User?.Email ?? "Unknown",
                CarId = order.CarId,
                CarBrand = order.Car?.Brand ?? "",
                CarModel = order.Car?.Model ?? "",
                Quantity = order.Quantity ?? 0,
                TotalAmount = order.TotalAmount,
                Status = order.Status ?? "Pending",
                OrderDate = order.OrderDate ?? DateTime.Now,
                NotificationType = "Created",
                Message = $"New order placed for {order.Car?.Brand} {order.Car?.Model}"
            };

            // Notify admins
            await _adminHubContext.Clients.Group("Admins").SendAsync("ReceiveOrderNotification", notification);

            // Notify the user who placed the order
            await _orderHubContext.Clients.Group($"user_{order.UserId}").SendAsync("ReceiveOrderConfirmation", notification);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _orderRepository.Update(order);
            await _orderRepository.SaveAsync();

            // Send SignalR notification about order update
            var notification = new OrderNotificationDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserName = order.User?.Email ?? "Unknown",
                CarId = order.CarId,
                CarBrand = order.Car?.Brand ?? "",
                CarModel = order.Car?.Model ?? "",
                Quantity = order.Quantity ?? 0,
                TotalAmount = order.TotalAmount,
                Status = order.Status ?? "Pending",
                OrderDate = order.OrderDate ?? DateTime.Now,
                NotificationType = "Updated",
                Message = $"Order #{order.OrderId} status changed to {order.Status}"
            };

            // Notify admins
            await _adminHubContext.Clients.Group("Admins").SendAsync("ReceiveOrderUpdate", notification);

            // Notify the user whose order was updated
            await _orderHubContext.Clients.Group($"user_{order.UserId}").SendAsync("ReceiveOrderUpdate", notification);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }
    }
}
