using Shoop.Models;

namespace Shoop.Data.Services
{
    public interface IShoppingCartService
    {
        Task<clsShoppingCart> GetByNameAsync(string Name);
        Task<clsShoppingCart> GetByIDAsync(int ID);

    }
}
