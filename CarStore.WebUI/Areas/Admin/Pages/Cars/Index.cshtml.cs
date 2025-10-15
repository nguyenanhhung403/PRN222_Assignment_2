using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.WebUI.Areas.Admin.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly ICarService _carService;

        public IndexModel(ICarService carService)
        {
            _carService = carService;
        }

        public IEnumerable<Car> Cars { get; set; }

        public async Task OnGetAsync()
        {
            Cars = await _carService.GetAllCarsAsync();
        }
    }
}
