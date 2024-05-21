using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Shoop.Data.Services;
using Shoop.Models;

namespace Shoop.Controllers
{
    public class GategoreController : Controller
    {
        private readonly IWebHostEnvironment enivrment;
        private readonly GategoreService service;
        public GategoreController(IWebHostEnvironment _enivrment, GategoreService _service)
        {
            enivrment = _enivrment;
            service = _service;
        }
        public IActionResult Index()
        {
            return View("Index");
        }


        [HttpPost]
        public async Task <IActionResult> ChangePhoto(int id ,string name,int number,IFormFile photo)
        {
            if(photo==null)
            {
                TempData["ErrorMessage"] = "Plese Choose Imgae.";
                return RedirectToAction("Index", "Admin");
            }

            string uploadsFolder = Path.Combine(enivrment.WebRootPath, "UplodeImages/GategoryImages");
            string uniqueFileName = photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            if (!System.IO.File.Exists(filePath))
            {

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
            }
            var gategory = await service.GetByIDAsync(id);

            if (gategory.Image != "/UplodeImages/GategoryImages/ImageNull.jpg")
            {
                string oldImagePath = Path.Combine(enivrment.WebRootPath, gategory.Image.Trim('/'));
                System.IO.File.Delete(oldImagePath);
            }
            gategory.NumberOfProduct = number;
            gategory.GategoryName = name;
            gategory.ID= id;
            gategory.Image = "/UplodeImages/GategoryImages/" + uniqueFileName;

            var Ruslt = await service.Update(gategory);
            if (Ruslt)
                TempData["SuccessMessage"] = "Data Updated successfully!";
            else
                TempData["ErrorMessage"] = "Plese Enter All filds above.";

            return RedirectToAction("Index", "Admin");
        }

   


    }
}
