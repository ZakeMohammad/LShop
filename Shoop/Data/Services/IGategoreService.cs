using Shoop.Models;

namespace Shoop.Data.Services
{
    public interface IGategoreService
    {
        Task<clsGategore> GetByIDAsync(int ID);
    }
}
