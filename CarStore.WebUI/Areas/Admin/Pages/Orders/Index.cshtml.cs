using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarStore.WebUI.Areas.Admin.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IEnumerable<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _orderService.GetAllOrdersAsync();
        }
    }
}
