using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shoop.Data.Services;
using Shoop.Models;
using Shoop.ViewModel;

namespace Shoop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService service;
        private readonly IWebHostEnvironment enivrment;
        private readonly GategoreService gategoreService;
        private readonly TrandyAndArrivedProductsService TrandArricedService;
        private readonly FavoriteService FavriteService;
		private readonly ReviewsService ReviewService;
		private readonly ShoppingCartService Shopcartservice;
		public ProductController(ProductService _service, ShoppingCartService shopcartservice, IWebHostEnvironment _enivrment,GategoreService _gategoreservice , TrandyAndArrivedProductsService trandyservice,FavoriteService favoutitesrevice,ReviewsService reviewservice)
        {
            TrandArricedService = trandyservice;
            service = _service;
            enivrment = _enivrment;
            gategoreService = _gategoreservice;
          FavriteService = favoutitesrevice;
            ReviewService = reviewservice;
			Shopcartservice=shopcartservice;

		}

        enum enProductGategory { Accessories=1, Cosmetics, Clothes, Shoes }
        enum AdminOrUser { Admin=1,User}

        List<string> _Images;
        public IActionResult Index()
        {
            return View("WelcomePage");
        }

        public IActionResult IndexForAdmin()
        {
            return View("WelcomePage");
        }

        public async Task< IActionResult> AllShoes(int id)
        {       
            var AllProduct = await service.GetAllByGategoryID((int)enProductGategory.Shoes);

            if (id == (int)AdminOrUser.Admin)
            {
                if (AllProduct.Count == 0)
                    return View("WelcomePage");

                return View("Index", AllProduct);
            }
            else
            {
                if (AllProduct.Count == 0)
                    return RedirectToAction("Index", "Home");

                return View("AllProductsForUser", AllProduct);
            }
        }
        public async Task<IActionResult> AllAccessories(int id)
        {         
            var AllProduct = await service.GetAllByGategoryID((int)enProductGategory.Accessories);


            if (id == (int)AdminOrUser.Admin)
            {
                if (AllProduct.Count == 0)
                    return View("WelcomePage");

                return View("Index", AllProduct);
            }
            else
            {
                if (AllProduct.Count == 0)
                    return RedirectToAction("Index", "Home");

                return View("AllProductsForUser", AllProduct);
            }
        }

        public async Task<IActionResult> AllCosmetics(int id)
        {            
            var AllProduct = await service.GetAllByGategoryID((int)enProductGategory.Cosmetics);


            if (id == (int)AdminOrUser.Admin)
            {
                if (AllProduct.Count == 0)
                    return View("WelcomePage");

                return View("Index", AllProduct);
            }
            else
            {
                if (AllProduct.Count == 0)
                    return RedirectToAction("Index", "Home");

                return View("AllProductsForUser", AllProduct);
            }
        }

        public async Task<IActionResult> AllClothes(int id)
        {        
            var AllProduct = await service.GetAllByGategoryID((int)enProductGategory.Clothes);
        

            if (id == (int)AdminOrUser.Admin)
            {
                if (AllProduct.Count == 0)
                    return View("WelcomePage");

                return View("Index", AllProduct);
            }
            else
            {
                if (AllProduct.Count == 0)
                    return RedirectToAction("Index","Home");

                return View("AllProductsForUser", AllProduct);
            }
               
         
        }


        [HttpGet]
        public IActionResult Create()=> View("CreateProduct");

		[HttpPost]
        public async Task<IActionResult> Create(clsProduct Product, IFormFile imagefile, IFormFile imagefile2, IFormFile imagefile3, IFormFile imagefile4, IFormFile imagefile5)
        {


            if (imagefile != null)
                Product.ImageOne = await GetImageURL(imagefile);
            else
                Product.ImageOne = "/UplodeImages/ProductImages/ImageNull.jpg";

            if (imagefile2 != null)
                Product.ImageTwo = await GetImageURL(imagefile2);
            else
                Product.ImageTwo = "/UplodeImages/ProductImages/ImageNull.jpg";

			if (imagefile3 != null)
                Product.ImageThere = await GetImageURL(imagefile3);
            else
                Product.ImageThere = "/UplodeImages/ProductImages/ImageNull.jpg";

			if (imagefile4 != null)
                Product.ImageFour = await GetImageURL(imagefile4);
            else
                Product.ImageFour = "/UplodeImages/ProductImages/ImageNull.jpg";

			if (imagefile5 != null)
                Product.ImageFive = await GetImageURL(imagefile5);
            else
                Product.ImageFive = "/UplodeImages/ProductImages/ImageNull.jpg";
            if (Product.Description == null)
                Product.Description = "null";

      
			try
                {
                    var Ruslt = await service.Add(Product);
                    if (Ruslt)
                    {

                       if( await gategoreService.IncrementNumberOfProduct(Product.CategoryID,true))
                     TempData["SuccessMessage"] = "Product added successfully!";

                      return  RedirectToAction("IndexForAdmin");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Plese Enter All Filds above.";

                    }
                }
                catch
                {
                    TempData["ErrorMessage"] = $"Unable to save changes.";

                }
           
          

            return RedirectToAction("IndexForAdmin");

        }


        public async Task<string> GetImageURL(IFormFile imagefile)
        {
            string uploadsFolder = Path.Combine(enivrment.WebRootPath, "UplodeImages/ProductImages");
            string uniqueFileName = imagefile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!System.IO.File.Exists(filePath))
            {

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagefile.CopyToAsync(stream);
                }
            }
            return "/UplodeImages/ProductImages/" + uniqueFileName;
        }


        public async Task<IActionResult> Detlis(int id)
        {
            var product = await service.GetByIDAsync(id);
            return View("PrdouctDeltilsForAdmin", product);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var Product = await service.GetByIDAsync(id);
            var GategoreID = Product.CategoryID;
            List<string> Images =
             [
                 Product.ImageOne,
                 Product.ImageTwo,
                 Product.ImageThere,
                 Product.ImageFour,
                 Product.ImageFive,
             ];
            DeleteImages(Images);
          
				if (await FavriteService.IsProductExist(id))
				{
					if (await FavriteService.DeleteProductFromFavouteByProductID(id))
					{
						
					}
					else
					{
						TempData["ErrorMessage"] = "Product deos not deleted.";
						return RedirectToAction("IndexForAdmin");
					}
				}
			if (await TrandArricedService.IsProductExist(id))
			{
				if (await TrandArricedService.DeleteProductFromTrandyByProductID(id))
				{

				}
				else
				{
					TempData["ErrorMessage"] = "Product deos not deleted.";
					return RedirectToAction("IndexForAdmin");
				}
			}

			if (await ReviewService.IsProductExist(id))
			{
				if (await ReviewService.DeleteProductFromReviewByProductID(id))
				{

				}
				else
				{
					TempData["ErrorMessage"] = "Product deos not deleted.";
					return RedirectToAction("IndexForAdmin");
				}
			}
			if (await Shopcartservice.IsProductExist(id))
			{
				if (await Shopcartservice.DeleteProductFromCartyProductID(id))
				{

				}
				else
				{
					TempData["ErrorMessage"] = "Product deos not deleted.";
					return RedirectToAction("IndexForAdmin");
				}
			}

			if (await service.Delete(Product))
			{
				if (await gategoreService.IncrementNumberOfProduct(GategoreID, false))
				{

					TempData["SuccessMessage"] = "Product deleted successfully!";

				}


				return RedirectToAction("IndexForAdmin");
			}
			else
			{
				TempData["ErrorMessage"] = "Product deos not deleted.";
				return RedirectToAction("IndexForAdmin");
			}
		}

		public IActionResult ConfirmThisAction(int id)
		{
			ConfirmParametrs model = new ConfirmParametrs();
			model.ID = id;
			model.ActoinName = "Delete";
			model.ActionForWhat = "Product";
			return View("ConfirmdView", model);
		}

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Product= await service.GetByIDAsync(id);
            List<string>Images=
            [
                Product.ImageOne,
                Product.ImageTwo,
                Product.ImageThere,
                Product.ImageFour,
                Product.ImageFive,
            ];
            DeleteImages(Images);
            return View("Update", Product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(clsProduct Product, IFormFile imagefile, IFormFile imagefile2, IFormFile imagefile3, IFormFile imagefile4, IFormFile imagefile5)
        {

           
            if (imagefile != null)
                Product.ImageOne = await GetImageURL(imagefile);
            else
                Product.ImageOne = "/UplodeImages/ProductImages/ImageNull.jpg";

            if (imagefile2 != null)
                Product.ImageTwo = await GetImageURL(imagefile2);
            else
                Product.ImageTwo = "/UplodeImages/ProductImages/ImageNull.jpg";

            if (imagefile3 != null)
                Product.ImageThere = await GetImageURL(imagefile3);
            else
                Product.ImageThere = "/UplodeImages/ProductImages/ImageNull.jpg";

            if (imagefile4 != null)
                Product.ImageFour = await GetImageURL(imagefile4);
            else
                Product.ImageFour = "/UplodeImages/ProductImages/ImageNull.jpg";

            if (imagefile5 != null)
                Product.ImageFive = await GetImageURL(imagefile5);
            else
                Product.ImageFive = "/UplodeImages/ProductImages/ImageNull.jpg";
            if (Product.Description == null)
                Product.Description = "null";

        
            try
            {

                var Ruslt = await service.Update(Product);
                if (Ruslt)
                {
                  

                    TempData["SuccessMessage"] = "Data Updated successfully!";
                
                    return RedirectToAction("IndexForAdmin");
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
			return RedirectToAction("IndexForAdmin");


		}


        [HttpPost]
        public async Task<IActionResult> SerachByName(string Name,int GategoryID)
        {
            if (Name.IsNullOrEmpty()|| Name == "")
            {
                TempData["ErrorMessage"] = "Plese Enter Name for search.";

                if(ApplicationUser.CurrentAdmin==null)
                return RedirectToAction("Index","Home");
                else
                    return RedirectToAction("IndexForAdmin");
            }
             
            
            if(GategoryID==(int)enProductGategory.Accessories)
            {
                var listofproduct = await service.SearchFor(Name, (int)enProductGategory.Accessories);
                if (listofproduct.Count != 0)
                {
                    if (ApplicationUser.CurrentAdmin == null)
                    {
                        string productListJson = JsonConvert.SerializeObject(listofproduct);                    
                        TempData["Products"] = productListJson;
                       
                        return RedirectToAction("FillterGategore", "UserInteractions");
                    }              
                       
                    else
                        return View("Index", listofproduct);                 
                }
             
                else
                {
                    TempData["ErrorMessageW"] = "No product like that name in system.";

                    if (ApplicationUser.CurrentAdmin == null)
                        return RedirectToAction("Index", "Home");
                    else
                    return RedirectToAction("IndexForAdmin");
                }

            }


            if (GategoryID == (int)enProductGategory.Cosmetics)
            {
                var listofproduct = await service.SearchFor(Name, (int)enProductGategory.Cosmetics);
                if (listofproduct.Count != 0)
                {
                    if (ApplicationUser.CurrentAdmin == null)
                    {
                        string productListJson = JsonConvert.SerializeObject(listofproduct);
                        TempData["Products"] = productListJson;

                        return RedirectToAction("FillterGategore", "UserInteractions");
                    }
                    else
                        return View("Index", listofproduct);
                }

                else
                {
                    TempData["ErrorMessageW"] = "No product like that name in system.";

                    if (ApplicationUser.CurrentAdmin == null)
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToAction("IndexForAdmin");
                }

            }


            if (GategoryID == (int)enProductGategory.Clothes)
            {
                var listofproduct = await service.SearchFor(Name, (int)enProductGategory.Clothes);
                if (listofproduct.Count != 0)
                {
                    if (ApplicationUser.CurrentAdmin == null)
                    {
                        string productListJson = JsonConvert.SerializeObject(listofproduct);
                        TempData["Products"] = productListJson;

                        return RedirectToAction("FillterGategore", "UserInteractions");
                    }
                    else
                        return View("Index", listofproduct);
                }

                else
                {
                    TempData["ErrorMessage"] = "No product like that name in system.";

                    if (ApplicationUser.CurrentAdmin == null)
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToAction("IndexForAdmin");
                }

            }


            if (GategoryID == (int)enProductGategory.Shoes)
            {
                var listofproduct = await service.SearchFor(Name, (int)enProductGategory.Shoes);
                if (listofproduct.Count != 0)
                {
                    if (ApplicationUser.CurrentAdmin == null)
                    {
                        string productListJson = JsonConvert.SerializeObject(listofproduct);
                        TempData["Products"] = productListJson;

                        return RedirectToAction("FillterGategore", "UserInteractions");
                    }
                    else
                        return View("Index", listofproduct);
                }

                else
                {
                    TempData["ErrorMessageW"] = "No product like that name in system.";

                    if (ApplicationUser.CurrentAdmin == null)
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToAction("IndexForAdmin");
                }

            }

            return RedirectToAction("IndexForAdmin");
        }

        private void DeleteImages(List<string> Images)
        {
            foreach (var Oldimage in Images)
            {
                if (Oldimage == "/UplodeImages/ProductImages/ImageNull.jpg")
                    continue;
                string oldImagePath = Path.Combine(enivrment.WebRootPath, Oldimage.Trim('/'));
                System.IO.File.Delete(oldImagePath);
            }
        }
     
        public async Task<IActionResult> AddProductToTrandyOrJustArrived(int id,bool IsTrandy)
        {
           var result = await service.GetByIDForTrandyandArivedProductsAsync(id, IsTrandy);

            if (result == null)
            {
                clsTrandyAndArrivedProducts Product = new clsTrandyAndArrivedProducts();
                Product.ProductID = id;
                Product.IsTrandy = IsTrandy;
                if (await TrandArricedService.AddProdutToTrandyorJustArrived(Product))
                    TempData["SuccessMessage"] = "Data Updated successfully!";
            }
            else
                TempData["ErrorMessage"] = "This Product is already in this section";

            return RedirectToAction("Index", "Admin");
        }
        public async Task<IActionResult> DeleteProductFromTrandyOrJustArrived(int id, bool IsTrandy)
        {
            var Product = await service.GetByIDForTrandyandArivedProductsAsync(id, IsTrandy);
            if (Product != null)
            {
                if(await TrandArricedService.Delete(Product))
                {
                    TempData["SuccessMessage"] = "Data Updated successfully!";
                }
            }
            else
                TempData["ErrorMessage"] = "This Product not in this section";
            return RedirectToAction("Index", "Admin");
        }

    }
}
