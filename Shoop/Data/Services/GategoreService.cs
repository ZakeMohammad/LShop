using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public class GategoreService : BaseRepositry<clsGategore>, IGategoreService
    {
        private readonly ApplicationDBContext _dbContext;
        public GategoreService(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> IncrementNumberOfProduct(int GategoreID,bool IsIncrement)
        {
            var Gategore = await GetByIDAsync(GategoreID);

          

            if (IsIncrement)
                Gategore.NumberOfProduct++;
            else
            {
                if (Gategore.NumberOfProduct == 0)
                    return true;
                Gategore.NumberOfProduct--;
            }
               

            _dbContext.Update(Gategore);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<clsGategore> GetByIDAsync(int ID) => await _dbContext.Set<clsGategore>().FirstOrDefaultAsync(x => x.ID == ID);
    }
}
