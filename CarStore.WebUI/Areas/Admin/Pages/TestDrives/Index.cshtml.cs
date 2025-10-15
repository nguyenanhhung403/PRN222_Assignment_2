using CarStore.BLL.Services;
using CarStore.BO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarStore.WebUI.Areas.Admin.Pages.TestDrives
{
    public class IndexModel : PageModel
    {
        private readonly ITestDriveService _testDriveService;
        public IndexModel(ITestDriveService testDriveService)
        {
            _testDriveService = testDriveService;
        }

        public IEnumerable<TestDrive> TestDrives { get; set; }

        public async Task OnGetAsync()
        {
            TestDrives = await _testDriveService.GetAllTestDrivesAsync();
        }
    }
}
