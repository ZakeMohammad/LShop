using Shoop.Models;

namespace Shoop.Data.Services
{
    public interface IAdminService
    {
        Task<clsAdmin> GetByNameAsync(string Name);
        Task<clsAdmin> GetByIDAsync(int ID);
        Task<clsAdmin> GetByEmailAsync(string email);
    }
}
