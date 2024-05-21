using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public class PaymentService : BaseRepositry<clsPayment>, IPaymentService
    {
        private readonly ApplicationDBContext _dbContext;

        public PaymentService(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<clsPayment> GetByIDAsync(int ID) => await _dbContext.Set<clsPayment>().FirstOrDefaultAsync(x => x.ID == ID);


        public Task<clsPayment> GetByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public Task<List<dynamic>> GitPaymentsDeltils()
        {
			var query = from payment in _dbContext.Payments
						join user in _dbContext.Users on payment.UserID equals user.ID
						join order in _dbContext.Orders on payment.OrderID equals order.ID
						join paymentMethod in _dbContext.PaymentMethods on payment.PaymentMethod equals paymentMethod.ID
						join city in _dbContext.Citys on payment.City equals city.ID
						join orderStatus in _dbContext.OrderStatus on order.Status equals orderStatus.ID
						where user.ID == order.UserID
						select new
						{
							ID = payment.ID,
							Name = user.Name,
							OrderDate = order.OrderDate,
							Date = payment.Date,
                            PaymentName = paymentMethod.PaymentName,
							CityName = city.CityName,
                            ShippingAdrees = payment.ShippingAdrees,
                            PhoneNumber = payment.PhoneNumber,
							Email = payment.Email,
                            Status = orderStatus.Status
						};

			return query.ToListAsync<object>();
		}


		public async Task<List<clsPayment>> SearchFor(int id) => await _dbContext.Set<clsPayment>().Where(x => x.ID == id).ToListAsync();

    }
}
