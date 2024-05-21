using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public class ReviewsService : BaseRepositry<clsReviews>
    {

        private readonly ApplicationDBContext _dbContext;
        public ReviewsService(ApplicationDBContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<List<clsReviews>> AllReviewsForThisProduct(int ProductID) => await _dbContext.Set<clsReviews>().Where(x => x.ProductID == ProductID).ToListAsync();

		public async Task<clsReviews> GetByID(int ID) => await _dbContext.Set<clsReviews>().FirstOrDefaultAsync(x => x.ID == ID);
		public async Task<List<dynamic>> GetAllReviewsDataForProduct(int ProductID)
        {
            var reviewData = from Reviews in _dbContext.Reviews
                             join Users in _dbContext.Users on Reviews.UserID equals Users.ID
                             join Products in _dbContext.Products on Reviews.ProductID equals Products.ID
                             where Products.ID == ProductID & Reviews.IsShow==true
							 select new
                             {
                                 Users.Name,
                                 UserImageURL = Users.ImageURL,
                                 ProductName = Products.Name,
                                 Reviews.Review,
                                 Reviews.Date,
                                 Reviews.Rating
                             };

            return await reviewData.ToListAsync<object>();
        }

		public async Task<bool> IsProductExist(int ProductID) => await _dbContext.Set<clsReviews>().AnyAsync(x => x.ProductID == ProductID);

		public async Task<bool> DeleteProductFromReviewByProductID(int ProductID)
		{
			var favouriteProducts = _dbContext.Reviews
		   .Where(fp => fp.ProductID == ProductID);

			if (!favouriteProducts.Any())
			{
				return false;
			}
			_dbContext.Reviews.RemoveRange(favouriteProducts);
			return await _dbContext.SaveChangesAsync() > 0;

		}




	}
}
