using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PROG6212_POE_PART_3_ST10150631.Models;
using PROG6212_POE_PART_3_ST10150631.ViewModels;
using System;

namespace PROG6212_POE_PART_3_ST10150631.Controllers
{
    public class SemesterController : Controller
    {
        SemesterViewModel SemViewModel = new SemesterViewModel();
        public IActionResult SemesterView()
        {
            if(MainViewModel.SignedInUser == null)
            {
                return View();
            }
            try
            {
                List<SemesterModel> semList = new List<SemesterModel>();
                semList = SemViewModel.GetAllSemesters(MainViewModel.SignedInUser);
                return View(semList);

            } catch
            {

            }
            return View();
        }

        public IActionResult AddSemesterView()
        {
            return View();
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> BtnDeleteClicked(string SemName)
        {

            try
            {
                await SemViewModel.DeleteSemesterAsync(SemName);
                return RedirectToAction("SemesterView"); // Redirect to Home/Index after successful deletion
            }
            catch 
            {
                TempData["ErrorMessage"] = "Delete all modules from a semester before deleting the semester ";
                return RedirectToAction("SemesterView"); // Redirect to Home/Index after successful deletion
            }
        }



        [HttpGet]
        public IActionResult AddSemesterClicked()
        {
            var model = new SemesterModel();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSemesterClicked(SemesterModel model, int Day, int Month, int Year)
        {
            if (model.SemesterName == null || model.Weeks == 0 )
            {
                return RedirectToAction("AddSemesterView");
            }
            if (MainViewModel.SignedInUser.IsNullOrEmpty())
            {
                return RedirectToAction("LoginView", "User");
            }
                // Combine the selected values into a DateTime object
                model.StartDate = new DateTime(Year, Month, Day);
                 await SemViewModel.AddSemesterToDBAsync(model);
                return RedirectToAction("SemesterView");

        }

            public async Task<IActionResult> ShowAllSemesters()
        {
            List<SemesterModel> semesters = new List<SemesterModel>();
            if(semesters.Count > 0)
            {
                return View(semesters);
            }
            else return View();
            
        }
    }
}
