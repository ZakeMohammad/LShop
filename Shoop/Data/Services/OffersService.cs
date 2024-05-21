using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public class OffersService : BaseRepositry<clsOffers>
    {
        private readonly ApplicationDBContext _dbContext;
        public OffersService(ApplicationDBContext dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<clsOffers> GetByIDAsync(int ID) => await _dbContext.Set<clsOffers>().FirstOrDefaultAsync(x => x.ID == ID);

    }
}
