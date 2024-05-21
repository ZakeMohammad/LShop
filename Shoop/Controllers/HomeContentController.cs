using Microsoft.AspNetCore.Mvc;
using Shoop.Data.Services;
using Shoop.Models;

namespace Shoop.Controllers
{
    public class HomeContentController : Controller
    {
       
      
        readonly private GategoreService _GategoreService;
        readonly private OffersService _OffersService;
        readonly private MessageService _messegesService;
        readonly private ReviewsService _reviewsService;
        readonly private ContactMessagesService _contactmessService;
        private readonly IWebHostEnvironment _enivrment;
        public HomeContentController(GategoreService gategoreservice,OffersService offersservice,IWebHostEnvironment _envirment, MessageService messegeservice,ReviewsService reviewservice,ContactMessagesService contactmesgeesservice)
        {
          
            _GategoreService = gategoreservice;
            _OffersService = offersservice;
            _enivrment = _envirment;
            _messegesService = messegeservice;
            _reviewsService = reviewservice;
            _contactmessService = contactmesgeesservice;
        }
        public IActionResult Index()
        {
            return View("Index");
        }
      
        public async Task<IActionResult> OffierAndNews() => View("OffierAndNews", await _OffersService.GetAll());



        public async Task<IActionResult> ManegeGategory()
        {
            var allGategory = await _GategoreService.GetAll();
            return View("ManegeGategory" , allGategory);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOffeir(string Name,string Detils, int GategoreID, int OfferRatio,IFormFile imagefile)
        {
            clsOffers offers = new clsOffers();
            offers.Name = Name;
            offers.Detils = Detils;
            offers.GategoreID = GategoreID;
            offers.OfferRatio = OfferRatio;

            if (imagefile != null)
                offers.Image = await GetImageURL(imagefile);
            else
                offers.Image = "/UplodeImages/OfferImages/ImageNull.jpg";

            try
            {
                var Ruslt = await _OffersService.Add(offers);
                if (Ruslt)
                {
                    TempData["SuccessMessage"] = "Offer added successfully!";

                   return RedirectToAction("OffierAndNews");
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
            return RedirectToAction("OffierAndNews");
        }

        public async Task<string> GetImageURL(IFormFile imagefile)
        {
            string uploadsFolder = Path.Combine(_enivrment.WebRootPath, "UplodeImages/OfferImages");
            string uniqueFileName = imagefile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!System.IO.File.Exists(filePath))
            {

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagefile.CopyToAsync(stream);
                }
            }
            return "/UplodeImages/OfferImages/" + uniqueFileName;
        }

        public async Task<IActionResult> DeleteOffire(int id)
        {
            var offer = await _OffersService.GetByIDAsync(id);


            if (offer.Image != "/UplodeImages/OfferImages/ImageNull.jpg")
            {
                string oldImagePath = Path.Combine(_enivrment.WebRootPath, offer.Image.Trim('/'));
                System.IO.File.Delete(oldImagePath);
            }
            if ( await _OffersService.Delete(offer))
                TempData["SuccessMessage"] = "Offer Deleted successfully!";
            else
                TempData["ErrorMessage"] = "Unable to Delete offer.";
            return RedirectToAction("OffierAndNews");
        }



      
    
        public async Task<IActionResult> AddNewMessege(string Message)
        {
            if (ApplicationUser.CurrentUser == null)
            {
                TempData["ErrorMessage"] = "You must log in to use this feature";
                return RedirectToAction("Index", "Home");
            }

            clsMeseges NewMessege =new clsMeseges();

            NewMessege.Message = Message;
            NewMessege.UserID = ApplicationUser.CurrentUser.ID;
            NewMessege.IsShow= true;
            NewMessege.Date=DateTime.Now;
            if (await _messegesService.Add(NewMessege))
                TempData["SuccessMessage"] = "Thanks for the comment!";
            else
                TempData["ErrorMessageW"] = "Message did not send, plese return sending message";
            return RedirectToAction("Index", "Home");


		}

        public async Task<IActionResult> DeleteMessege(int id)
        {
          if( await _messegesService.Delete(await _messegesService.GetByID(id)))
                TempData["SuccessMessage"] = "Message deleted";
            else
                TempData["ErrorMessageW"] = "Message did not delete";
            return RedirectToAction("Index", "Admin");
        }
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (await _reviewsService.Delete(await _reviewsService.GetByID(id)))
                TempData["SuccessMessage"] = "Review Deleted";
            else
                TempData["ErrorMessageW"] = "Review did not delete";
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> AllMessegesAdmin() => View("AllMesegesAdmin", await _messegesService.GetAll());
        public async Task<IActionResult> AllReview() => View("AllReview", await _reviewsService.GetAll());

        public async Task<IActionResult> AllContactMessages()
		{
			ViewData["Title"] = "Messages";
			return View("AllContactMessages", await _contactmessService.GetAll());
        }
		public async Task<IActionResult> DeleteContactMessage(int id)
		{
			if (await _contactmessService.Delete(await _contactmessService.GetByID(id)))
				TempData["SuccessMessage"] = "Contact Message Deleted";
			else
				TempData["ErrorMessageW"] = "Contact Message did not delete";
			return RedirectToAction("Index", "Admin");
		}


        public async Task<IActionResult> AddContactMessage(string Name,string Subject, string Message, string Email)
        {
            clsContactMessages NewMessage=new clsContactMessages();
            NewMessage.Name = Name;
            NewMessage.Subject = Subject;
            NewMessage.Message = Message;
            NewMessage.Email = Email;
			if (await _contactmessService.Add(NewMessage))
				TempData["SuccessMessage"] = "Contact Message Added ";
			else
				TempData["ErrorMessageW"] = "Contact Message did not Add";
			return RedirectToAction("Index", "Home");
		}

	}
}
