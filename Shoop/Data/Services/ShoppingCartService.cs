using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public class ShoppingCartService:BaseRepositry<clsShoppingCart>, IShoppingCartService
    {
        private readonly ApplicationDBContext _dbContext;
    

        public ShoppingCartService(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<clsShoppingCart> GetByIDAsync(int CartID) => await _dbContext.Set<clsShoppingCart>().FirstOrDefaultAsync(x => x.ID == CartID);
        public async Task<clsShoppingCart> GetCartForUserByIDAsync(int UserID) => await _dbContext.Set<clsShoppingCart>().FirstOrDefaultAsync(x => x.UserID == UserID);
        public async Task<bool> IsProductExist(int ProductID) => await _dbContext.Set<clsShoppingCart>().AnyAsync(x => x.ProductID == ProductID);
        public Task<clsShoppingCart> GetByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }


        public async Task<decimal> GetTotalAmount(int UserID)
        {
            var totalAmountForCart = _dbContext.ShoppingCarts
                  .Join(_dbContext.Users, sc => sc.UserID, u => u.ID, (sc, u) => new { sc, u })
                  .Join(_dbContext.Products, s => s.sc.ProductID, p => p.ID, (s, p) => new { s.sc, s.u, p })
                  .Where(x => x.u.ID == UserID)
                  .Select(x => new
                  {
                     TotalAmount = x.p.Price * x.sc.Quantity
                  })
                   .Sum(x => x.TotalAmount);

            return  totalAmountForCart;
        }


        public async Task<List<dynamic>> GetCartForThisUser(int UserID)
        {

            var result = await _dbContext.ShoppingCarts
                .Join(_dbContext.Users, sc => sc.UserID, u => u.ID, (sc, u) => new { sc, u })
                .Join(_dbContext.Products, s => s.sc.ProductID, p => p.ID, (s, p) => new { s.sc, s.u, p })
                .Where(x => x.u.ID == UserID) 
                .Select(x => new
                {
                   UserID= x.u.ID,
                    x.sc.ID,
                 ProductID= x.p.ID,
                    x.p.Name,
                    x.p.Price,
                    x.sc.Quantity,
                    TotalAmount = x.p.Price * x.sc.Quantity
                })
                .ToListAsync();
            return result.Cast<dynamic>().ToList(); 
        }

        public async Task<bool> AddProductToCart(int ProductID)
        {
           clsShoppingCart Cart=new clsShoppingCart();

            Cart.ProductID = ProductID;
            Cart.UserID = ApplicationUser.CurrentUser.ID;
            Cart.Quantity = 1;
            _dbContext.Add(Cart);

            return await _dbContext.SaveChangesAsync() > 0;
            
        }


        public async Task<bool> IncreaseProductQuantity(clsShoppingCart Cart  , bool IsIncreae)
        {

            if(IsIncreae)
                Cart.Quantity++;
            else
                Cart.Quantity--;
            _dbContext.Update(Cart);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> Delete(int CartID)
        {
            var Cart = await GetByIDAsync(CartID);

            _dbContext.Remove(Cart);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAllCartForThisUser(int UserID)
        {
            var shoppingCartsToDelete = _dbContext.ShoppingCarts
                     .Where(cart => cart.UserID == UserID);

            _dbContext.ShoppingCarts.RemoveRange(shoppingCartsToDelete);
           return _dbContext.SaveChanges() >0;
        }


	
		public async Task<bool> DeleteProductFromCartyProductID(int ProductID)
		{
			var favouriteProducts = _dbContext.ShoppingCarts
		   .Where(fp => fp.ProductID == ProductID);

			if (!favouriteProducts.Any())
			{
				return false;
			}
			_dbContext.ShoppingCarts.RemoveRange(favouriteProducts);
			return await _dbContext.SaveChangesAsync() > 0;

		}




	}
}
