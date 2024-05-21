using Shoop.Models;

namespace Shoop.Data.Services
{
    public interface IProductService
    {
      
        Task<clsProduct> GetByNameAsync(string Name);
        Task<clsProduct> GetByIDAsync(int ID);
    }
}
