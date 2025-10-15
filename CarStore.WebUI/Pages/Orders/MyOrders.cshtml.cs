using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarStore.WebUI.Pages.Orders
{
    [Authorize]
    public class MyOrdersModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public MyOrdersModel(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        public IEnumerable<Order> Orders { get; set; } = new List<Order>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Challenge();
            }

            var users = await _userService.GetAllUsersAsync();
            var currentUser = users.FirstOrDefault(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            Orders = await _orderService.GetOrdersByUserIdAsync(currentUser.UserId);

            return Page();
        }
    }
}
