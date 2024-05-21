using Microsoft.AspNetCore.Mvc;
using Shoop.Data.Services;
using Shoop.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace Shoop.Controllers
{
    public class UserInteractionsController : Controller
    {
        private readonly FavoriteService _favoriteService;
        private readonly ProductService _productService;
        private readonly ReviewsService _reviewsService;
        public UserInteractionsController(FavoriteService favioriteservice, ProductService productservice, ReviewsService reviewsService)
        {
            _favoriteService = favioriteservice;
            _productService = productservice;
            _reviewsService = reviewsService;
        }
        enum enProductGategory { Accessories = 1, Cosmetics, Clothes, Shoes }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddProductToFavorite(int ProductID)
        {
            TempData["ErrorMessage"] = null;
            TempData["ErrorMessageI"] = null;
            if (ApplicationUser.CurrentUser == null)
            {
                TempData["ErrorMessage"] = "You must log in to use this feature";
                return RedirectToAction("Index", "Home");
            }

            if(await _favoriteService.IsExist(ProductID,ApplicationUser.CurrentUser.ID))
            {
                TempData["ErrorMessageI"] = "This Product already exist in your favorites!";
                return RedirectToAction("Index", "Home");
            }

            clsFavorite NewFavorite= new clsFavorite();
            NewFavorite.ProductID = ProductID;
            NewFavorite.UserID = ApplicationUser.CurrentUser.ID;


            if(await _favoriteService.Add(NewFavorite))
            {
                TempData["SuccessMessage"] = "Product Add successfully to Favorites";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessageW"] = "Product did not added to Favorites";
                return RedirectToAction("Index", "Home");
            }

        }



        public async Task<IActionResult> ShowFavorite()
        {
            TempData["ErrorMessage"] = null;
            TempData["ErrorMessageW"] = null;
            if (ApplicationUser.CurrentUser == null)
            {
                TempData["ErrorMessage"] = "You must log in to use this feature";
                return RedirectToAction("Index", "Home");
            }

            var allFavirote = await _favoriteService.GetAllForThisUser(ApplicationUser.CurrentUser.ID);
            List<clsProduct> ProductList = new List<clsProduct>();
            if (allFavirote != null)
            {
                foreach(var item in  allFavirote)
                {
                    var Product = await _productService.GetByIDAsync(item.ProductID);
                    ProductList.Add(Product);
                }
                return View("MyFavorite", ProductList);
            }
            else
            {
                TempData["ErrorMessageI"] = "You dont have any product in your Favirote!";
                return RedirectToAction("Index", "Home");
            }
          
        }
       
        public async Task<IActionResult> Remove(int id)
        {

            if(await _favoriteService.Delete(await _favoriteService.GetByIDAsync(id)))
                TempData["SuccessMessage"] = "Product removed form favorites!";
            else
                TempData["ErrorMessage"] = "Product did not removed form favorites!";
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> ShowAllProductForGategore(int id)
        {
            var AllProductForThisGategory = await _productService.GetAllByGategoryID(id);

            if (AllProductForThisGategory != null)
                return View("AllProductForGatgegore", AllProductForThisGategory);
            else
                TempData["ErrorMessageI"] = "There are no products in this category";
            return RedirectToAction("Index", "Home");
        }



        public IActionResult FillterGategore()
        {
            string? productListJson = TempData["Products"] as string;
         
            List<clsProduct> listOfProducts = JsonConvert.DeserializeObject<List<clsProduct>>(productListJson);

            return View("AllProductForGatgegore", listOfProducts);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(clsReviews Review)
        {
            TempData["ErrorMessageW"] = null;
            TempData["ErrorMessageI"] = null;
            Review.Date = DateTime.Now;
            Review.IsShow = true;
            if(Review.Review.IsNullOrEmpty())
            {
                TempData["ErrorMessageW"] = "Plese make sure you write accepted review.";
                return RedirectToAction("Index", "Home");
            }
              

            if (await _reviewsService.Add(Review))
                TempData["ErrorMessageI"] = "Review Added successfully!";
            else
                TempData["ErrorMessageW"] = "Your Review did not added";
            return RedirectToAction("Index", "Home");

        }


        


    }
}
