using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public class FavoriteService : BaseRepositry<clsFavorite>
    {
        private readonly ApplicationDBContext _dbContext;
        public FavoriteService(ApplicationDBContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<clsFavorite> GetByIDAsync(int ID) => await _dbContext.Set<clsFavorite>().FirstOrDefaultAsync(x => x.ProductID == ID);
        public async Task<bool> IsExist(int ProductID,int UserID) => await _dbContext.Set<clsFavorite>().AnyAsync(x => x.ProductID == ProductID && x.UserID==UserID);
		public async Task<bool> IsProductExist(int ProductID) => await _dbContext.Set<clsFavorite>().AnyAsync(x => x.ProductID == ProductID);
		public async Task<bool> DeleteProductFromFavouteByProductID(int ProductID)
        {
            var favouriteProducts = _dbContext.Favorites
           .Where(fp => fp.ProductID == ProductID);

            if (!favouriteProducts.Any())
            {
                return false;
            }

         
            _dbContext.Favorites.RemoveRange(favouriteProducts);
          return  await _dbContext.SaveChangesAsync()>0;

           
        }
        public async Task<List<clsFavorite>> GetAllForThisUser(int UserID) => await _dbContext.Set<clsFavorite>().Where(x => x.UserID == UserID).ToListAsync();


    }
}
