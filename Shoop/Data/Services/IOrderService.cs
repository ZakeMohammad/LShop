using Shoop.Models;

namespace Shoop.Data.Services
{
    public interface IOrderService
    {
        Task<clsOrder> GetByNameAsync(string Name);
        Task<clsOrder> GetByIDAsync(int ID);

    }
}
