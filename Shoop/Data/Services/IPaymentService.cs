using Shoop.Models;

namespace Shoop.Data.Services
{
    public interface IPaymentService
    {
        Task<clsPayment> GetByNameAsync(string Name);
        Task<clsPayment> GetByIDAsync(int ID);
    }
}
