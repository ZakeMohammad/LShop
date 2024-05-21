using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public class ContactMessagesService : BaseRepositry<clsContactMessages>
    {
        private readonly ApplicationDBContext _dbContext;
        public ContactMessagesService(ApplicationDBContext context) : base(context)
        {
            _dbContext = context;
        }


        public async Task<clsContactMessages> GetByID(int ID) => await _dbContext.Set<clsContactMessages>().FirstOrDefaultAsync(x => x.ID == ID);
    }
}
