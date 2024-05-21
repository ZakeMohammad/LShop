using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
	public class TrandyAndArrivedProductsService : BaseRepositry<clsTrandyAndArrivedProducts>
	{
		private readonly ApplicationDBContext _dbContext;
		public TrandyAndArrivedProductsService(ApplicationDBContext context) : base(context)
		{
			_dbContext = context;
		}

		public async Task<bool> IsProductExistBeforeInSameSection(int ProductID, bool IsTrandy) =>await  _dbContext.Set<clsTrandyAndArrivedProducts>().AnyAsync(x => x.ProductID == ProductID && x.IsTrandy == IsTrandy);
		
		
		public async Task<List<dynamic>> GetTrandyProduct()
		{
			var result =  from p in _dbContext.Products
						 join t in _dbContext.TrandyAndArrivedProducts
						 on p.ID equals t.ProductID
						 where t.IsTrandy==true
						 select new
						 {
							 p.ID,
							 p.Name,
							 p.Description,
							 p.ImageOne,
							 p.ImageTwo,
							 p.ImageThere, 
							 p.ImageFour,
							 p.ImageFive,
							 p.Price,
							 p.CategoryID,
							 p.Quantity,
							 p.Rating,
							 t.IsTrandy
						 };		
			return await result.ToListAsync<object>();
		}
        public async Task<List<dynamic>> GetJustArrivadProduct()
        {
            var result = from p in _dbContext.Products
                         join t in _dbContext.TrandyAndArrivedProducts
                         on p.ID equals t.ProductID
                         where t.IsTrandy == false
                         select new
                         {
                             p.ID,
                             p.Name,
                             p.Description,
                             p.ImageOne,
                             p.ImageTwo,
                             p.ImageThere,
                             p.ImageFour,
                             p.ImageFive,
                             p.Price,
                             p.CategoryID,
                             p.Quantity,
                             p.Rating,
                             t.IsTrandy
                         };
            return await result.ToListAsync<object>();
        }


        public async Task<bool> AddProdutToTrandyorJustArrived(clsTrandyAndArrivedProducts Product)
        {
             _dbContext.Add(Product);
            return await _dbContext.SaveChangesAsync()>0;
        }

        public async Task<bool> DeleteProdutFromTrandyorJustArrived(clsTrandyAndArrivedProducts Product)
        {
            _dbContext.Remove(Product);
            return await _dbContext.SaveChangesAsync() > 0;
        }



		public async Task<bool> IsProductExist(int ProductID) => await _dbContext.Set<clsTrandyAndArrivedProducts>().AnyAsync(x => x.ProductID == ProductID);

		public async Task<bool> DeleteProductFromTrandyByProductID(int ProductID)
		{
			var favouriteProducts = _dbContext.TrandyAndArrivedProducts
		   .Where(fp => fp.ProductID == ProductID);

			if (!favouriteProducts.Any())
			{
				return false;
			}
			_dbContext.TrandyAndArrivedProducts.RemoveRange(favouriteProducts);
			return await _dbContext.SaveChangesAsync() > 0;

		}


	}
}
