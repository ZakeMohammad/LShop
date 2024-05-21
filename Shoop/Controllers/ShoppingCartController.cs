using Microsoft.AspNetCore.Mvc;
using Shoop.Data.Services;
using Shoop.Models;
using Shoop.ViewModel;

namespace Shoop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;
        public ShoppingCartController(ShoppingCartService Shoppcartservice) { _shoppingCartService = Shoppcartservice; }

  
        public async Task<IActionResult> ShowMyCart()
        {
            if (ApplicationUser.CurrentUser == null)
            {
                TempData["ErrorMessage"] = "You must log in to use this feature";
                return RedirectToAction("Index", "Home");
            }

            ShoppingCartModelView Model=new ShoppingCartModelView();
            
            Model.CartsDetlis = await _shoppingCartService.GetCartForThisUser(ApplicationUser.CurrentUser.ID);
            Model.TotalCartAmount = await _shoppingCartService.GetTotalAmount(ApplicationUser.CurrentUser.ID);
            return View("ShoppincCart", Model); 
        }
    
        public async Task<IActionResult> IncreaseAndDecrease(int id,string IsIncrease)
        {
            if (ApplicationUser.CurrentUser == null)
            {
                TempData["ErrorMessage"] = "You must Login to use this fuather";
                return RedirectToAction("Index", "Home");
            }


            var Card = await _shoppingCartService.GetByIDAsync(id);

            if (Card.Quantity == 1 && IsIncrease=="No")
            {
               if(await _shoppingCartService.Delete(Card))
               {
                    TempData["ErrorMessage"] = "Product Removed from your cart!";
                    return RedirectToAction("Index", "Home");
               }
                return RedirectToAction("Index", "Home");
            }

            if (IsIncrease == "Yes")
                if (await _shoppingCartService.IncreaseProductQuantity(Card, true))
                    return RedirectToAction("ShowMyCart");

            if (IsIncrease == "No")
                if (await _shoppingCartService.IncreaseProductQuantity(Card, false))
                    return RedirectToAction("ShowMyCart");


            return RedirectToAction("ShowMyCart");

        }


        public async Task<IActionResult> Delete(int id)=> await _shoppingCartService.Delete(id) ? RedirectToAction("ShowMyCart") : RedirectToAction("Index", "Home");


        public async Task<IActionResult> AddToCart(int id)
        {
            if (ApplicationUser.CurrentUser == null)
            {
                TempData["ErrorMessage"] = "You must log in to use this feature";
                return RedirectToAction("Index", "Home");
            }


            if (await _shoppingCartService.AddProductToCart(id))
                TempData["SuccessMessage"] = "Product Added to your shopping cart!";
            else
                TempData["ErrorMessageW"] = "Product did not add to your shopping cart!";

            return RedirectToAction("Index", "Home");
        }

    }
}
