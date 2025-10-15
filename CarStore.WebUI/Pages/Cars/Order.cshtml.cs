using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace CarStore.WebUI.Pages.Cars
{
    [Authorize]
    public class OrderModel : PageModel
    {
        private readonly ICarService _carService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderModel(ICarService carService, IOrderService orderService, IUserService userService)
        {
            _carService = carService;
            _orderService = orderService;
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public Car Car { get; set; }

        public class InputModel
        {
            public int CarId { get; set; }
            // The Quantity property is no longer needed here as it's hardcoded to 1.
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Car = await _carService.GetCarByIdAsync(id);
            if (Car == null)
            {
                return NotFound();
            }
            Input = new InputModel { CarId = id };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Since the form no longer sends CarId, we need to get it from the route.
            if (!int.TryParse(RouteData.Values["id"]?.ToString(), out int carId))
            {
                return BadRequest();
            }
            Input.CarId = carId;

            Car = await _carService.GetCarByIdAsync(Input.CarId);
            if (Car == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var users = await _userService.GetAllUsersAsync();
            var currentUser = users.FirstOrDefault(u => u.Email == userEmail);

            if (currentUser == null)
            {
                // This should not happen if user is authenticated
                return Challenge();
            }
            
            var order = new Order
            {
                CarId = Input.CarId,
                UserId = currentUser.UserId,
                Quantity = 1, // Hardcode quantity to 1
                TotalAmount = Car.Price * 1 // Calculate total amount with quantity of 1
            };

            await _orderService.CreateOrderAsync(order);

            return RedirectToPage("/Orders/MyOrders");
        }
    }
}
