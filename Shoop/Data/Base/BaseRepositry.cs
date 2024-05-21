using Microsoft.EntityFrameworkCore;
using System;

namespace Shoop.Data.Base
{
    public class BaseRepositry<T> where T : class
    {
        private readonly ApplicationDBContext _context;

        public BaseRepositry(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<bool> Add(T Entity)
        {
            _context.Set<T>().Add(Entity);
            var result = await _context.SaveChangesAsync();
			return result > 0 ? true : false;
		}

        public async Task<bool> Delete(T Entity)
        {
            _context.Set<T>().Remove(Entity);
			return await _context.SaveChangesAsync()>0;
        }

        public async Task<bool> Update(T Entity)
        {
            _context.Set<T>().Update(Entity);
            int changesSaved = await _context.SaveChangesAsync();

            return changesSaved > 0 ? true : false;
        }

        public async Task<IEnumerable<T>> GetAll() => await _context.Set<T>().ToListAsync();

   
    }
}
