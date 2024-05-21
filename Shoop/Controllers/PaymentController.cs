using Microsoft.AspNetCore.Mvc;
using Shoop.Data.Services;
using Shoop.Models;
using Shoop.ViewModel;

namespace Shoop.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService service;
		private readonly OrderService OrderService;
        private readonly UserService UserService;
        public PaymentController(PaymentService _service, OrderService _orderservice,UserService _userservice)
        {
            service = _service;
            OrderService = _orderservice;
			UserService = _userservice;
        }

		enum enPaymentMethod { Cash=1, Card}
		enum enOrderStatus { Painding = 1, Active, Done }
		public async Task< IActionResult>Index()
        {
            return View("Index", await service.GitPaymentsDeltils());
        }

        public async Task<IActionResult> SerachBy(int ID)
        {
			TempData["ErrorMessage"] = null;

			if (ID == 0 || ID < 0)
            {
                TempData["ErrorMessage"] = "No Payments like this ID!";

                return RedirectToAction("Index", "Admin");
            }
            else
                return View("Index", await service.SearchFor(ID));

        }

  
		public IActionResult ConfirmThisAction(int id)
		{
			ConfirmParametrs model = new ConfirmParametrs();
			model.ID = id;
			model.ActoinName = "Delete";
			model.ActionForWhat = "Payment";
			return View("ConfirmdView", model);
		}


		public async Task<IActionResult> Delete(int id)
		{

			if (await service.Delete(await service.GetByIDAsync(id)))
			{

				TempData["SuccessMessage"] = "Payment deleted successfully!";

				return RedirectToAction("Index", "Admin");
			}
			else
			{
				TempData["ErrorMessage"] = "Payment deos not deleted.";
				return RedirectToAction("Index", "Admin");
			}
		}


		[HttpPost]
		public async Task<IActionResult> PayCash(clsPayment PaymentData)
		{
			
			PaymentData.PaymentMethod = (int)enPaymentMethod.Cash;

			if(await service.Add(PaymentData))
			{
				if(await OrderService.ChangeOrderStatus(PaymentData.OrderID, (int)enOrderStatus.Active))
				{
					if(await UserService.IncreaseNumberOfPurshees(PaymentData.UserID))
					{
                        TempData["SuccessMessage"] = "Thank you, the delivery company will contact you for more information about delivering the order.";
                        return RedirectToAction("Index", "Home");
                    }
				
				}
			}
			TempData["ErrorMessage"] = "There is Somthing wrong , please return the payment";
			return RedirectToAction("Index", "Home");
		}







	}
}
