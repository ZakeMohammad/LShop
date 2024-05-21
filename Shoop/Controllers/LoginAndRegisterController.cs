using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Shoop.Data.Services;
using Shoop.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shoop.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly IUserSerevice UserService;
        private readonly IAdminService AdminService;
        public LoginAndRegisterController(IUserSerevice _userservice, IAdminService adminService)
        {
            UserService = _userservice;
            AdminService = adminService;

        }
        public IActionResult Index()
        {
            return View();
        }



        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute hash from the password
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static bool VerifyPassword(string storedHash, string userInput)
        {
            string hashedInput = HashPassword(userInput);
            return storedHash.Equals(hashedInput);
        }

        [HttpGet]
        public IActionResult AddNewUserByRegister()
		{
			ViewData["Title"] = "Login Page";
			return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUserByRegisterAsync2(clsUser NewUser)
        {
            TempData["ErrorMessage"] = null;
            TempData["SuccessMessage"] = null;
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please enter all fields!";
                return View("Login");
            }
              

            var IsThereOntherUserlikeThisEmail = await UserService.IsExist(NewUser.Email);

            if (IsThereOntherUserlikeThisEmail)
            {
                TempData["ErrorMessage"] = "There another user like this email!";
                return RedirectToAction("AddNewUserByRegister");
            }
             


            NewUser.Password = HashPassword(NewUser.Password);
            var Result = await UserService.Add(NewUser);

            if (Result)
                return View("registerDone", NewUser);
            else
            {
                TempData["ErrorMessage"] = "Regiser not work!";
                return RedirectToAction("AddNewUserByRegister");
            }
              

        }

        [HttpGet]
        public IActionResult Login()
		{
			ViewData["Title"] = "Login Page";
	    	return	View("Login");

        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync2(string Email, string Password)
        {
            TempData["ErrorMessage"] = null;
            TempData["SuccessMessage"] = null;
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ViewBag.ShowDiv = true;
                TempData["ErrorMessage"] = "Please enter email and password!";
                return RedirectToAction("Login");
            }
            var IsAdminExist = await AdminService.GetByEmailAsync(Email);

            if (IsAdminExist != null)
            {
                if (VerifyPassword(IsAdminExist.Password, Password))
                {
                    if (IsAdminExist.IsActive != true)
                    {

						ViewData["Title"] = "Not Active";
						return View("NotActiveView");
                    }

                    ApplicationUser.CurrentAdmin = IsAdminExist;

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    TempData["ErrorMessage"] = "Password is wrong, please try again!";
                }
              
            }
            else
            {
                var IsUserExist = await UserService.GetByEmailAsync(Email);

                if (IsUserExist != null)
                {
                    if (VerifyPassword(IsUserExist.Password, Password))
                    {
                        if (IsUserExist.IsActive != true)
                        {
                            return View("NotActiveView");
                        }
                        ApplicationUser.CurrentUser = IsUserExist;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                       
                        TempData["ErrorMessage"] = "Password is wrong, please try again!";
                    }


                }
            }
            return RedirectToAction("Login");
        }




        [HttpGet]
        public IActionResult LogOut()
        {
            ApplicationUser.CurrentUser = null;
            ApplicationUser.CurrentAdmin = null;
      
            return RedirectToAction("Index","Home");
        }






    }
}
