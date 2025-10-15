using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.WebUI.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly ICarService _carService;

        public IndexModel(ICarService carService)
        {
            _carService = carService;
        }

        public IEnumerable<Car> Cars { get; set; } = new List<Car>();

        public async Task OnGetAsync()
        {
            Cars = await _carService.GetAllCarsAsync();
        }
    }
}
