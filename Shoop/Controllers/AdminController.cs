using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shoop.Data.Services;
using Shoop.Models;
using Shoop.ViewModel;
using System.Security.Cryptography;
using System.Text;

namespace Shoop.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminService service;
        private readonly IWebHostEnvironment enivrment;

        public AdminController(AdminService _service,IWebHostEnvironment _eniv)
        {
            service = _service;
            enivrment = _eniv;
        }

        public IActionResult Index()
        {
            return View("WelcomPage");
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
        public async Task<IActionResult> AllAdmins() => View("Index", await service.GetAll());

		[HttpGet]
		public IActionResult Create() => View("Add");



		[HttpPost]
        public async Task<IActionResult> Create(clsAdmin admin, IFormFile imagefile)
        {
            if (imagefile != null)
            {

                string uploadsFolder = Path.Combine(enivrment.WebRootPath, "UplodeImages/AdminImages");
                string uniqueFileName = imagefile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!System.IO.File.Exists(filePath))
                {

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagefile.CopyToAsync(stream);
                    }
                }



                admin.ImageURL = "/UplodeImages/AdminImages/" + uniqueFileName;
                admin.IsActive = true;
                admin.IsAdmin = true;
                admin.Password = HashPassword(admin.Password);
                var isexist = await service.IsExist(admin.Email);
                if (isexist)
                {
                    TempData["ErrorMessage"] = "This Email is already has account.";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {

                        var Ruslt = await service.Add(admin);
                        if (Ruslt)
                        {
                            TempData["SuccessMessage"] = "Admin added successfully!";

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
        public async Task<IActionResult> Update(int id)
        {
            var admin = await service.GetByIDAsync(id);
            if (admin == null)
                return RedirectToAction("Index", "Admin");
            string oldImagePath = Path.Combine(enivrment.WebRootPath, admin.ImageURL.Trim('/'));
            System.IO.File.Delete(oldImagePath);
            return View("Update", admin);
        }
       

        [HttpPost]
        public async Task<IActionResult> Update(clsAdmin admin, IFormFile imagefile)
        {
            if (imagefile != null)
            {
              
                string uploadsFolder = Path.Combine(enivrment.WebRootPath, "UplodeImages/AdminImages");
                string uniqueFileName = imagefile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!System.IO.File.Exists(filePath))
                {

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagefile.CopyToAsync(stream);
                    }
                }
                admin.ImageURL = "/UplodeImages/AdminImages/" + uniqueFileName;

                admin.IsAdmin = true;
                admin.IsActive = true;


                try
                {
                    var Ruslt = await service.Update(admin);
                    if (Ruslt)
                    {
                      
                        TempData["SuccessMessage"] = "Data Updated successfully!";

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
            TempData["ErrorMessage"] = "Unable to save changes.";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var admin = await service.GetByIDAsync(id);
            if (admin == null)
                return RedirectToAction("Index", "Admin");
            string OldImage = admin.ImageURL;
            if(OldImage!= "/UplodeImages/AdminImages/ImageNull.jpg")
            {
                string oldImagePath = Path.Combine(enivrment.WebRootPath, OldImage.Trim('/'));
                System.IO.File.Delete(oldImagePath);
            }
          
            if (await service.Delete(admin))
            {
                TempData["SuccessMessage"] = "Admin deleted successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "This Admin does not deleted because he is active in system";
                return RedirectToAction("Index");
            }
        }

 
        public async Task<IActionResult> ActiveThis(int id)
        {
            var admin = await service.GetByIDAsync(id);
            if (admin == null)
                return RedirectToAction("Index", "Admin");
            if (admin.IsActive)
            {

                if (await service.ActiveThisAdmin(admin))
                    TempData["SuccessMessage"] = "Data Updated successfully!";
                else
                    TempData["ErrorMessage"] = "Data Does not saved!";

                return RedirectToAction("Index");
            }
            else
            {
                if (await service.ActiveThisAdmin(admin))
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
            model.ActionForWhat = "Admin";
            return View("ConfirmdView", model);
        }

		[HttpPost]
		public async Task<IActionResult> SerachByName(string Name)
		{
			if (Name.IsNullOrEmpty())
				return RedirectToAction("Index");
            else
				return View("Index", await service.SearchForAdmin(Name));
		}




	}
}
