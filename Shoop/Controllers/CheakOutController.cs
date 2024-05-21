using Microsoft.AspNetCore.Mvc;
using Shoop.Data.Services;
using Shoop.Models;
using Shoop.ViewModel;

namespace Shoop.Controllers
{
    public class CheakOutController : Controller
    {
        private readonly OrderService Orderservice;
		private readonly OrderDetailsService Orderdetilsservice;
		public CheakOutController( OrderService _orderservice, OrderDetailsService _detilsservice)
        {
            Orderservice= _orderservice;
            Orderdetilsservice= _detilsservice;
        }
        public async Task< IActionResult> Index()
        {
            if (ApplicationUser.CurrentUser == null)
            {
                TempData["ErrorMessage"] = "You must log in to use this feature";
                return RedirectToAction("Index", "Home");
            }

            var Orders= await Orderservice.GetOrdersIDForThisUserNotPayment(ApplicationUser.CurrentUser.ID);

            if(Orders.Count == 0)
            {
                TempData["ErrorMessageI"] = "Plese make sure you must have order to use this feature";
                return RedirectToAction("Index", "Home");
            }
            CheakOutViewModel model = new CheakOutViewModel();

            model.OrdersData = new List<List<dynamic>>();   
			for (int i = 0; i < Orders.Count; i++)
			{
				var result = await Orderservice.GetAllitemsForThisOrder(Orders[i]);
				model.OrdersData.Add(result);
			}
			return View("Index",model);
        }


        public async Task<IActionResult> PayByCash(int OrderID)
        {
            PaymentViewModel model= new PaymentViewModel();

            model.OrderDetlis = new List<dynamic>();
			var result = await Orderservice.GetAllitemsForThisOrder(OrderID);
            if (result == null)
                return RedirectToAction("Index", "Home");
            model.OrderDetlis.Add(result);
			model.TotalAmount = 0;
            model.TotalAmount = await Orderservice.GetTotalAmountForThisOrder(OrderID);

            return View("PaymentForThisOrderByCash", model);
        }
	


		public async Task<IActionResult> DeleteOrder(int OrderID)
        {
            if(await Orderdetilsservice.DeleteOrder(OrderID))
            {
                if(await Orderservice.Delete(await Orderservice.GetByIDAsync(OrderID)))
				TempData["SuccessMessage"] = "Order deleted successfully!";
			}			
            else
				TempData["ErrorMessage"] = "Order did not deleted";


			return RedirectToAction("Index", "Home");

		}

	}
}
