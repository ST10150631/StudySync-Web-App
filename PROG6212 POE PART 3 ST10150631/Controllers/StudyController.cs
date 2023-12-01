using Microsoft.AspNetCore.Mvc;
using PROG6212_POE_PART_3_ST10150631.ViewModels;

namespace PROG6212_POE_PART_3_ST10150631.Controllers
{
    public class StudyController : Controller
    {
        ModuleViewModel moduleViewModel = new ModuleViewModel();
        public IActionResult Index()
        {
            if (MainViewModel.SignedInUser == null) { return View(); }
            var list = moduleViewModel.GetAllModules();
            if (moduleViewModel.GetAllModules() == null) { return View(); }
            foreach ( var module in list )
            {
                module.ProgressBarValue = moduleViewModel.CalculatePercentage(module.CompletedSelfHrs, module.WeeklySelfHrs);
            }
            return View(list);
        }

        [HttpPost,ActionName("StartNewWeek")]
        public async Task<IActionResult> StartNewWeek()
        {
            if (MainViewModel.SignedInUser == null) 
            {
                return RedirectToAction("Index");
            }
            else

            await moduleViewModel.ResetHrsStudied(MainViewModel.SignedInUser);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddHoursStudied(string  hrs, string ModuleName)
        {
            if(double.TryParse(hrs, out var hours))
            {
                await moduleViewModel.AddHrsStudied(ModuleName, hours);
                RedirectToAction("Index");
            }
            else
            
                TempData["ErrorMessage"] = "Invalid input for hours studied.";
                return RedirectToAction("Index");
            
            
        }
    }
}
