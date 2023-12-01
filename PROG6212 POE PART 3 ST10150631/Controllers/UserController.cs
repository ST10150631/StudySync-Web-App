using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PROG6212_POE_PART_3_ST10150631.ViewModels;
using PROG6212_POE_PART_3_ST10150631.Models;

namespace PROG6212_POE_PART_3_ST10150631.Controllers
{
    public class UserController : Controller
    {
        private UserViewModel ViewModel = new UserViewModel();
        private UserModel model = new UserModel();

        public ActionResult LoginView()
        {
            return View();
        }
        public ActionResult RegisterView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var response = new UserViewModel();
            return View();
        }

        /// <summary>
        /// This method will check the users registration is valid 
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Register(UserModel user)
        {
            bool anyFieldBlank = string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.Surname);

            if (anyFieldBlank)
            {
                // If any field is blank, set a flag in ModelState
                ModelState.AddModelError("", "Please fill in all fields");
                return View("RegisterView", user);
            }
            else if (!ModelState.IsValid) return View(user);

            if (await ViewModel.RegisterUser(user.Username, user.FirstName, user.Surname, user.PasswordHash))
            {
                MainViewModel.SignedInUser = user.Username;
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return View("RegisterView");
            }
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new UserViewModel();
            return View();
        }

        /// <summary>
        /// This method will check the users registration is valid 
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel user)
        {
            bool anyFieldBlank = string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.PasswordHash);

            if (anyFieldBlank)
            {
                // If any field is blank, set a flag in ModelState
                ModelState.AddModelError("", "Please fill in all fields");
                return View("LoginView", user);
            }
            if (!ModelState.IsValid) return View(user);

            if (await ViewModel.LoginUser(user.Username,user.PasswordHash))
            {
                MainViewModel.SignedInUser = user.Username;
                return RedirectToAction("Index", "Home");
            }
            else return View("LoginView");

        }
    }
}
