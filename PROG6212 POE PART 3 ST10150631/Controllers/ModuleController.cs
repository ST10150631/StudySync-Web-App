using Microsoft.AspNetCore.Mvc;
using PROG6212_POE_PART_3_ST10150631.ViewModels;
using PROG6212_POE_PART_3_ST10150631.Models;

namespace PROG6212_POE_PART_3_ST10150631.Controllers
{
    public class ModuleController : Controller
    {
        ModuleViewModel ViewModel = new ModuleViewModel();
        SemesterViewModel semesterViewModel = new SemesterViewModel();
        public ModuleController()
        {
            
        }
        public IActionResult ModuleView ()
        {
            
            if(MainViewModel.SignedInUser == null) { return View(); }
            var list = ViewModel.GetAllModules ();
            if(ViewModel.GetAllModules () == null) { return View(); }
            return View(list);
        }
        public IActionResult AddModuleView()
        {
            if(MainViewModel.SignedInUser == null) { return View("ModuleView"); }
            ModuleModel moduleModel = new ModuleModel();
            moduleModel.SemesterList =  semesterViewModel.GetAllSemesters(MainViewModel.SignedInUser);
            if (moduleModel.SemesterList == null) { return View("ModuleView"); }
            return View(moduleModel);
        }

        [HttpGet]
        public IActionResult AddModuleClicked() 
        { 
            return RedirectToAction("ModuleView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModuleClicked(ModuleModel module,string SemesterName)
        {
            module.SemesterName = SemesterName;
            await ViewModel.AddNewModule(module);
            
            return RedirectToAction("ModuleView");
        }

        [HttpGet]
        public IActionResult BtnDeleteClicked()
        {
            return View();
        }

        [HttpPost, ActionName("BtnDeleteModule")]
        public async Task<IActionResult> BtnDeleteModule(string ModName)
        {
            try
            {
                await ViewModel.DeleteModuleAsync(ModName);
                return RedirectToAction("ModuleView");
            }
            catch
            {
                TempData["ErrorMessage"] = "Delete all modules from a semester before deleting the semester ";
                return RedirectToAction("ModuleView");
            }
        }
    }
}
