using Shoop.Models;

namespace Shoop.Data.Services
{
    public interface IOrderDetailsService
    {
        Task<clsOrderDetails> GetByNameAsync(string Name);
        Task<clsOrderDetails> GetByIDAsync(int ID);
    }
}
