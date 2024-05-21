using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shoop.Data.Services;
using Shoop.Models;
using Shoop.ViewModel;
using System.Security.Cryptography;
using System.Text;

namespace Shoop.Controllers
{
	public class UserController : Controller
    {
        private readonly UserService service;
        private readonly IWebHostEnvironment enivrment;
        public UserController(UserService _service, IWebHostEnvironment _enivrment)
        {
            service = _service;
            enivrment = _enivrment;

        }
        public async Task<IActionResult> Index() => View("Index", await service.GetAll());

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
        [HttpGet]
        public IActionResult Create() => View("Add");

        


        [HttpPost]
        public async Task<IActionResult> Create(clsUser User, IFormFile imagefile)
        {
            if (imagefile != null)
            {
              
                string uploadsFolder = Path.Combine(enivrment.WebRootPath, "UplodeImages/UsersImages");
                string uniqueFileName = imagefile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!System.IO.File.Exists(filePath))
                {
                  
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagefile.CopyToAsync(stream);
                    }
                }



                User.ImageURL = "/UplodeImages/UsersImages/" + uniqueFileName;
                User.IsActive = true;
                User.IsAdmin = false;
                User.Password = HashPassword(User.Password);
                var isexist = await service.IsExist(User.Email);
                if (isexist)
                {
                    TempData["ErrorMessage"] = "This Email is already has account.";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {

                        var Ruslt = await service.Add(User);
                        if (Ruslt)
                        {
                            TempData["SuccessMessage"] = "User added successfully!";

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Plese Enter All Filds above.";

                        }
                    }
                    catch
                    {
                        TempData["ErrorMessage"] = "Unable to save changes.";

                    }
                    return RedirectToAction("Index");

                }


            }
            TempData["ErrorMessage"] = "Unable to save changes.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task< IActionResult> Update(int id)=> View("Update", await service.GetByIDAsync(id));

        [HttpPost]
        public async Task<IActionResult> Update(clsUser User, IFormFile imagefile)
        {
            if (imagefile != null)
            {
                string OldImage = ApplicationUser.CurrentUser.ImageURL;
                string uploadsFolder = Path.Combine(enivrment.WebRootPath, "UplodeImages/UsersImages");
                string uniqueFileName = imagefile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!System.IO.File.Exists(filePath))
                {

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagefile.CopyToAsync(stream);
                    }
                }
                User.ImageURL = "/UplodeImages/UsersImages/" + uniqueFileName;
             
                User.IsAdmin = false;
                User.IsActive = true;
               
             
                    try
                    {

                    if (ApplicationUser.CurrentUser.Email != User.Email && await service.IsExist(User.Email))
                    {
                        TempData["ErrorMessage"] = "This Email is already has account.";
                        return RedirectToAction("AllAdmins");
                    }



                    var Ruslt = await service.Update(User);
                        if (Ruslt)
                        {
                        string oldImagePath = Path.Combine(enivrment.WebRootPath, OldImage.Trim('/'));
                        System.IO.File.Delete(oldImagePath);
                            
                        
                        TempData["SuccessMessage"] = "Data Updated successfully!";

                            return RedirectToAction("Index","Home");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Plese Enter All Filds above.";

                        }
                    }
                    catch
                    {
                        TempData["ErrorMessage"] = "Unable to save changes.";

                    }
                    return RedirectToAction("Index", "Home");
            }
            TempData["ErrorMessage"] = "Unable to save changes.";
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Delete (int id)
        {
            var user = await service.GetByIDAsync(id);
            if (user.ImageURL != "/UplodeImages/UsersImages/ImageNull.jpg")
            {
                string oldImagePath = Path.Combine(enivrment.WebRootPath, user.ImageURL.Trim('/'));
                System.IO.File.Delete(oldImagePath);
            }

            if (await service.Delete(user))
            {
                TempData["SuccessMessage"] = "User deleted successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "This user does not deleted because he is active in system";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detlis(int id) => View("Detalis", await service.GetByIDAsync(id));




        public async Task<IActionResult> ActiveThis(int id)
        {
            var user= await service.GetByIDAsync(id);
            if (user.IsActive)
            {

                if( await service.ActiveThisUser(user))
                    TempData["SuccessMessage"] = "Data Updated successfully!";
                else
                    TempData["ErrorMessage"] = "Data Does not saved!";

                return RedirectToAction("Index");
            }
            else
            {
                if (await service.ActiveThisUser(user))
                    TempData["SuccessMessage"] = "Data Updated successfully!";
                else
                    TempData["ErrorMessage"] = "Data Does not saved!";

                return RedirectToAction("Index");
            }
          
        }

  
		public IActionResult ConfirmThisAction(string ActionName, int id)
		{
			ConfirmParametrs model = new ConfirmParametrs();
			model.ID = id;
			model.ActoinName = ActionName;
			model.ActionForWhat = "User";
			return View("ConfirmdView", model);
		}



		public IActionResult ChangePassword() => View("ChangePassword");

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string CurrentPass,string NewPassword,string ConfirmedNewPassword)
        {
            

            if (NewPassword != ConfirmedNewPassword)
            {
                TempData["ErrorMessage"] = "New Password And Confirmed does not same!";
                return RedirectToAction("Index", "Home");
            }

            if (HashPassword(CurrentPass) == ApplicationUser.CurrentUser.Password)
            {
                if( await service.ChangePassword(ApplicationUser.CurrentUser, NewPassword))
                    TempData["SuccessMessage"] = "Password change successfully!";
                else
                    TempData["ErrorMessage"] = "Password does not changed!";

            }
            else
            {
                TempData["ErrorMessage"] = "Current Password Wrong!";
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SerachByNameUsers(string Name)
        {
            if (Name.IsNullOrEmpty())
                return RedirectToAction("Index");
            else
                return View("Index", await service.SearchForUsers(Name)); 
        }
      
    

    }



}
