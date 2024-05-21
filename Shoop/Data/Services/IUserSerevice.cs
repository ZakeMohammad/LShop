using Shoop.Data.Base;
using Shoop.Models;

namespace Shoop.Data.Services
{
    public interface IUserSerevice 
    {
        Task<clsUser> GetByNameAsync(string Name);
        Task<clsUser> GetByIDAsync(int ID);
        Task<clsUser> GetByEmailAsync(string email);
        Task<bool> IsExist(string email);

        Task<bool>Add(clsUser user);
    }
}
