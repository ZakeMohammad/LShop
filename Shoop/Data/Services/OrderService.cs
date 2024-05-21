using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;
using System.ComponentModel;

namespace Shoop.Data.Services
{
    public class OrderService: BaseRepositry<clsOrder>, IOrderService
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderService(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        enum enOrderStatus { Painding=1, Active, Done }

        public async Task<clsOrder> GetByIDAsync(int ID) => await _dbContext.Set<clsOrder>().FirstOrDefaultAsync(x => x.ID == ID);
       

        public Task<clsOrder> GetByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetLatestOrderID()=> await _dbContext.Orders.OrderByDescending(o => o.ID).Select(o => o.ID).FirstOrDefaultAsync();


		public async Task<List<int>> GetOrdersIDForThisUserNotPayment(int UserID) => await _dbContext.Orders.Where(order => order.Status == (int)enOrderStatus.Painding && order.UserID == UserID)
			   .Select(order => order.ID)
			   .ToListAsync();


		public async Task<List<clsOrder>> SearchFor(int id) => await _dbContext.Set<clsOrder>().Where(x => x.ID==id).ToListAsync();

        public async Task<List<dynamic>> GetAllitemsForThisOrder(int OrderID)
        {
            var result = from orderDetail in _dbContext.OrderDetails
                         join product in _dbContext.Products on orderDetail.ProductID equals product.ID
                         join orders in _dbContext.Orders on orderDetail.OrderID equals orders.ID
                         where orderDetail.OrderID == OrderID
                         select new
                         {
                            ID=  orderDetail.ID,
                            OrderID=  orderDetail.OrderID,
                            OrderDate= orders.OrderDate,
                            Name = product.Name,
                            Quantity=  orderDetail.Quantity,
                            TotalPrice=  orderDetail.TotalPrice
                         };
            return await result.ToListAsync<object>(); 
        }


        public async Task<decimal> GetTotalAmountForThisOrder(int OrderID) => (from orderDetail in _dbContext.OrderDetails
																			   join product in _dbContext.Products on orderDetail.ProductID equals product.ID
																			   where orderDetail.OrderID == OrderID
																			   select orderDetail.TotalPrice).Sum();


		public async Task<List<clsOrder>> GitAllOrderForThisUser(int UserID)=> await _dbContext.Set<clsOrder>().Where(x => x.UserID==UserID).ToListAsync();

        public async Task<bool> ChangeOrderStatus(int OrderID, int OrderStatus)
        {
            var Order= await GetByIDAsync(OrderID);
            Order.Status = OrderStatus;
            _dbContext.Update(Order);

            return await _dbContext.SaveChangesAsync() > 0;
        }



        public async Task<bool> UserPaymentForThisOrder(int OrderID)
        {
            var Order= await GetByIDAsync(OrderID);

            Order.Status = (int)enOrderStatus.Done;

            _dbContext.Update(Order);

            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
