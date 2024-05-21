
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Shoop.Data.Services;
using Shoop.Models;
using Shoop.ViewModel;
using System.Collections;
using System.Collections.Generic;

namespace Shoop.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService service;
        private readonly UserService serviceuser;
        private readonly OrderDetailsService serviceorderdetilss;
        private readonly ShoppingCartService shoppingservice;
        public OrderController(OrderService _service, UserService _userservice,ShoppingCartService _shoppingservice, OrderDetailsService _orderdetilsservice)
        {
            service = _service;
          serviceuser = _userservice;
            shoppingservice = _shoppingservice;
            serviceorderdetilss = _orderdetilsservice;
        }
		enum enOrderStatus { Painding = 1, Active, Done }

		public async Task<IActionResult> Index()
        {
            var AllOrdesr = await service.GetAll();

            return View("Index", AllOrdesr);
        }

        public async Task<IActionResult> ShowOrderDeltislAdmin(int id)
        {
            var order = await service.GetByIDAsync(id);
            var user = await serviceuser.GetByIDAsync(order.UserID);
            Dictionary<clsOrder, clsUser> Params = new Dictionary<clsOrder, clsUser>();

            Params.Add(order, user);
            return  View("OrderDeltislAdmin", Params);
        }


        public async Task<IActionResult> ShowOrderItemsDeltislAdmin(int id)
        {

            var Detils=await service.GetAllitemsForThisOrder(id);
            return View("OrderItems", Detils);
        }
        public async Task<IActionResult> Delete(int id)
		{

			if (await service.Delete(await service.GetByIDAsync(id)))
			{
				TempData["SuccessMessage"] = "Order deleted successfully!";

				return RedirectToAction("Index","Admin");
			}
			else
			{
				TempData["ErrorMessage"] = "Order deos not deleted.";
				return RedirectToAction("Index", "Admin");
			}
		}


		public IActionResult ConfirmThisAction(int id)
		{
			ConfirmParametrs model = new ConfirmParametrs();
			model.ID = id;
			model.ActoinName = "Delete";
			model.ActionForWhat = "Order";
			return View("ConfirmdView", model);
		}

		public async Task<IActionResult> SerachBy(int UserID)
        {
            if (UserID == null || UserID == 0 || UserID<0)
            {
                TempData["ErrorMessage"] = "No Users like this id has ordres!";
               
                return RedirectToAction("Index", "Admin");
            }               
            else
                return View("Index", await service.SearchFor(UserID));

        }

        public async Task<IActionResult> ConfirmOrder(int userid,decimal totalamount)
        {
            TempData["ErrorMessage"] = null;
            if (totalamount == 0)
            {
                TempData["ErrorMessage"] = "Plese Choose Products before make order!";

                return RedirectToAction("Index", "Home");
            }

            clsOrder NewOrder = new clsOrder();

            NewOrder.UserID = userid;
            NewOrder.OrderDate = DateTime.Now;
            NewOrder.Status =(int)enOrderStatus.Painding ;
            NewOrder.TotalAmount = totalamount;
            int OrderID = 0;
            if (await service.Add(NewOrder))
            {
                OrderID = await service.GetLatestOrderID();
            }
            else
                return RedirectToAction("Index", "Home");

            List<dynamic> CartDataForOrderItems = await shoppingservice.GetCartForThisUser(userid);


            foreach (var item in CartDataForOrderItems)
            {
                clsOrderDetails NewOrderDetlis= new clsOrderDetails();
              
                NewOrderDetlis.OrderID = OrderID;
                NewOrderDetlis.ProductID = item.ProductID;
                NewOrderDetlis.UnitPrice = item.Price;
                NewOrderDetlis.TotalPrice = item.TotalAmount;
                NewOrderDetlis.Quantity=item.Quantity;
                await serviceorderdetilss.Add(NewOrderDetlis);
            }
            if (await shoppingservice.DeleteAllCartForThisUser(userid))
            {

              return RedirectToAction("Index", "Home");
            }
            else
                 return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> UserPaymentForThisOrder(int OrderID)
        {
            if(await service.UserPaymentForThisOrder(OrderID))
                TempData["SuccessMessage"] = "Order Updated successfully!";
            else
                TempData["ErrorMessage"] = "Order did not updated!";

            return RedirectToAction("Index", "Admin");
        }

    }
}
