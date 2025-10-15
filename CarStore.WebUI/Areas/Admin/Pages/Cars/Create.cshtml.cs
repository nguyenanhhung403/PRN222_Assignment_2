using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CarStore.WebUI.Areas.Admin.Pages.Cars
{
    public class CreateModel : PageModel
    {
        private readonly ICarService _carService;

        public CreateModel(ICarService carService)
        {
            _carService = carService;
        }

        [BindProperty]
        public Car Car { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _carService.CreateCarAsync(Car);

            return RedirectToPage("./Index");
        }
    }
}
