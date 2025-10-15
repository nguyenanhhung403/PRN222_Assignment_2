using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarStore.WebUI.Areas.Admin.Pages.TestDrives
{
    public class EditModel : PageModel
    {
        private readonly ITestDriveService _testDriveService;

        public EditModel(ITestDriveService testDriveService)
        {
            _testDriveService = testDriveService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public TestDrive TestDrive { get; set; }

        public SelectList StatusOptions { get; set; }
        
        public class InputModel
        {
            public int TestDriveId { get; set; }
            [Required]
            public string Status { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TestDrive = await _testDriveService.GetTestDriveByIdAsync(id.Value);

            if (TestDrive == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                TestDriveId = TestDrive.TestDriveId,
                Status = TestDrive.Status
            };

            var statuses = new List<string> { "Scheduled", "Done", "Canceled" };
            StatusOptions = new SelectList(statuses);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TestDrive = await _testDriveService.GetTestDriveByIdAsync(Input.TestDriveId);
                if (TestDrive == null)
                {
                    return NotFound();
                }
                var statuses = new List<string> { "Scheduled", "Done", "Canceled" };
                StatusOptions = new SelectList(statuses);
                return Page();
            }

            var testDriveToUpdate = await _testDriveService.GetTestDriveByIdAsync(Input.TestDriveId);

            if (testDriveToUpdate == null)
            {
                return NotFound();
            }

            testDriveToUpdate.Status = Input.Status;
            
            await _testDriveService.UpdateTestDriveAsync(testDriveToUpdate);

            return RedirectToPage("./Index");
        }
    }
}
