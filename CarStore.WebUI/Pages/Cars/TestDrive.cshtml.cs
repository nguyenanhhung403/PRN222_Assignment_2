using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace CarStore.WebUI.Pages.Cars
{
    [Authorize]
    public class TestDriveModel : PageModel
    {
        private readonly ICarService _carService;
        private readonly ITestDriveService _testDriveService;
        private readonly IUserService _userService;

        public TestDriveModel(ICarService carService, ITestDriveService testDriveService, IUserService userService)
        {
            _carService = carService;
            _testDriveService = testDriveService;
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public Car Car { get; set; }

        public class InputModel
        {
            public int CarId { get; set; }
            [Required]
            [FutureDate]
            [Display(Name = "Scheduled Date")]
            public DateTime ScheduleDate { get; set; } = DateTime.Now.AddDays(1);
            [MaxLength(255)]
            public string Note { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Car = await _carService.GetCarByIdAsync(id);
            if (Car == null) return NotFound();
            Input = new InputModel { CarId = id };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Car = await _carService.GetCarByIdAsync(Input.CarId);
            if (Car == null) return NotFound();
            if (!ModelState.IsValid) return Page();

            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var users = await _userService.GetAllUsersAsync();
            var currentUser = users.FirstOrDefault(u => u.Email == userEmail);
            if (currentUser == null) return Challenge();

            var testDrive = new TestDrive
            {
                CarId = Input.CarId,
                UserId = currentUser.UserId,
                ScheduleDate = Input.ScheduleDate,
                Note = Input.Note
            };

            await _testDriveService.ScheduleTestDriveAsync(testDrive);
            return RedirectToPage("/Index"); // Or a success page
        }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (DateTime)value > DateTime.Now;
        }
    }
}
