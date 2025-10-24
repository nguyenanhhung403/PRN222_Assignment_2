using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarStore.WebUI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.WebUI.Areas.Admin.Pages.TestDrives
{
    public class IndexModel : PageModel
    {
        private readonly ITestDriveService _testDriveService;
        private readonly ITestDriveNotificationService _notificationService;
        
        public IndexModel(ITestDriveService testDriveService, ITestDriveNotificationService notificationService)
        {
            _testDriveService = testDriveService;
            _notificationService = notificationService;
        }

        public IEnumerable<TestDrive> TestDrives { get; set; }

        [BindProperty]
        public UpdateStatusModel UpdateStatus { get; set; }

        public class UpdateStatusModel
        {
            public int TestDriveId { get; set; }
            public string Status { get; set; }
        }

        public async Task OnGetAsync()
        {
            TestDrives = await _testDriveService.GetAllTestDrivesAsync();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync()
        {
            var testDrive = await _testDriveService.GetTestDriveByIdAsync(UpdateStatus.TestDriveId);
            if (testDrive != null)
            {
                testDrive.Status = UpdateStatus.Status;
                await _testDriveService.UpdateTestDriveAsync(testDrive);
                
                // Gửi thông báo real-time
                await _notificationService.NotifyTestDriveUpdatedAsync(testDrive);
            }
            
            return RedirectToPage();
        }
    }
}
