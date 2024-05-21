using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public class ProductService : BaseRepositry<clsProduct>, IProductService
    {
        private readonly ApplicationDBContext _dbContext;
 
        private readonly TrandyAndArrivedProductsService TrandArricedService;
        public ProductService(ApplicationDBContext dbContext, TrandyAndArrivedProductsService trandyservice) : base(dbContext)
        {
            _dbContext = dbContext;
          
            TrandArricedService = trandyservice;

        }

        public async Task<clsProduct> GetByIDAsync(int id) => await _dbContext.Set<clsProduct>().FirstOrDefaultAsync(x => x.ID == id);

        public async Task<clsTrandyAndArrivedProducts> GetByIDForTrandyandArivedProductsAsync(int id,bool IsTrandy) => await _dbContext.Set<clsTrandyAndArrivedProducts>().FirstOrDefaultAsync(x => x.ProductID == id && x.IsTrandy==IsTrandy);
        public Task<clsProduct> GetByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public async Task<List<clsProduct>> GetAllByGategoryID(int ID) => await _dbContext.Set<clsProduct>().Where(x => x.CategoryID == ID).ToListAsync();

        public async Task<List<clsProduct>> SearchFor(string Name, int id) => await _dbContext.Set<clsProduct>().Where(x => x.Name.Contains(Name.Trim()) && x.CategoryID == id).ToListAsync();


        public async Task<dynamic> TrandyProduct()=>await TrandArricedService.GetTrandyProduct();

        public async Task<dynamic> JustArrivad() => await TrandArricedService.GetJustArrivadProduct();

  
	}

}