using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarStore.WebUI.Areas.Admin.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly IOrderService _orderService;

        public EditModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public Order Order { get; set; }

        public SelectList StatusOptions { get; set; }

        public class InputModel
        {
            public int OrderId { get; set; }
            [Required]
            public string Status { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _orderService.GetOrderByIdAsync(id.Value);

            if (Order == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                OrderId = Order.OrderId,
                Status = Order.Status
            };

            var statuses = new List<string> { "Pending", "Completed", "Canceled" };
            StatusOptions = new SelectList(statuses);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // If validation fails, we need to reload the full Order object for display
                Order = await _orderService.GetOrderByIdAsync(Input.OrderId);
                if (Order == null)
                {
                    return NotFound();
                }
                var statuses = new List<string> { "Pending", "Completed", "Canceled" };
                StatusOptions = new SelectList(statuses);
                return Page();
            }

            var orderToUpdate = await _orderService.GetOrderByIdAsync(Input.OrderId);

            if (orderToUpdate == null)
            {
                return NotFound();
            }

            orderToUpdate.Status = Input.Status;

            await _orderService.UpdateOrderAsync(orderToUpdate);

            return RedirectToPage("./Index");
        }
    }
}
