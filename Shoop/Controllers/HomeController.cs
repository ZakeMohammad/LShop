using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shoop.Data.Services;
using Shoop.Models;
using Shoop.ViewModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace Shoop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OffersService offersService;
        private readonly ProductService productservice;
        private readonly GategoreService gategorservice;
        private readonly ReviewsService reviewservice;
        private readonly MessageService messegesreview;
        public HomeController(ILogger<HomeController> logger,OffersService _serviceOffer, ProductService _serviceProduct,GategoreService _gategoerservice, ReviewsService _reviewservice,MessageService _messegesrevice)
        {
            _logger = logger;
           offersService = _serviceOffer;
            productservice = _serviceProduct;
            gategorservice = _gategoerservice;
            reviewservice = _reviewservice;
            messegesreview = _messegesrevice;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel Model= new HomeViewModel();

            Model.AllOffers = (List<clsOffers>)await offersService.GetAll();           
            Model.Gategores = (List<clsGategore>)await gategorservice.GetAll();
            Model.ArrivedProduct = await productservice.JustArrivad();
            Model.TrandyProduct = await productservice.TrandyProduct();
            Model.AllCommints = await messegesreview.GetMessegesDetils();
			return View("Index", Model);
        }
        public async Task<IActionResult> Detials(int id)
        {
            ProductDetilsViewModel model= new ProductDetilsViewModel(reviewservice);
            model.Product = await productservice.GetByIDAsync(id);
            model.clsProducts = await productservice.GetAllByGategoryID(model.Product.CategoryID);
            model.AllReviews = await reviewservice.GetAllReviewsDataForProduct(id);
           return  View("Detials", model);
        }

        public IActionResult Contact()
        {
            return View("Contact");
        }

  

    }
}
