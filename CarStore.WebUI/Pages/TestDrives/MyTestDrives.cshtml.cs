using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarStore.WebUI.Pages.TestDrives
{
    [Authorize]
    public class MyTestDrivesModel : PageModel
    {
        private readonly ITestDriveService _testDriveService;
        private readonly IUserService _userService;

        public MyTestDrivesModel(ITestDriveService testDriveService, IUserService userService)
        {
            _testDriveService = testDriveService;
            _userService = userService;
        }

        public IEnumerable<TestDrive> TestDrives { get; set; } = new List<TestDrive>();

        [BindProperty]
        public CancelInputModel Input { get; set; }

        public class CancelInputModel
        {
            [Required]
            public int TestDriveId { get; set; }
            [Required(ErrorMessage = "Please provide a reason for cancellation.")]
            [StringLength(200, MinimumLength = 5, ErrorMessage = "Reason must be between 5 and 200 characters.")]
            public string Note { get; set; }
        }

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

            TestDrives = await _testDriveService.GetTestDrivesByUserIdAsync(currentUser.UserId);

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            if (!ModelState.IsValid)
            {
                // To refresh the main page content if validation fails
                await OnGetAsync(); 
                return Page();
            }

            var testDrive = await _testDriveService.GetTestDriveByIdAsync(Input.TestDriveId);

            if (testDrive == null)
            {
                return NotFound();
            }
            
            if (testDrive.Status == "Scheduled")
            {
                testDrive.Status = "Canceled";
                testDrive.Note = Input.Note;
                await _testDriveService.UpdateTestDriveAsync(testDrive);
            }

            return RedirectToPage();
        }
    }
}
