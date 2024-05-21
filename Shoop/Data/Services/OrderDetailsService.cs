using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public class OrderDetailsService : BaseRepositry<clsOrderDetails>, IOrderDetailsService
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderDetailsService(ApplicationDBContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;
        }

        public async Task<List<clsOrderDetails>> AllProductForThisOrder(int ProductID) => await _dbContext.Set<clsOrderDetails>().Where(x => x.ProductID == ProductID).ToListAsync();

        public async Task<clsOrderDetails> GetByIDAsync(int ID) => await _dbContext.Set<clsOrderDetails>().FirstOrDefaultAsync(x => x.ID == ID);


        public Task<clsOrderDetails> GetByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> DeleteOrder(int OrderID)
        {
			
			var orderItems = await _dbContext.OrderDetails
										  .Where(od => od.OrderID ==OrderID)
										  .ToListAsync();

            if (orderItems.Any())
            {
				_dbContext.OrderDetails.RemoveRange(orderItems);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else return false;
		}

            
        




    }
}