using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shoop.Data.Services
{
    public class AdminService : BaseRepositry<clsAdmin>, IAdminService
    {
        private readonly ApplicationDBContext _dbContext;

        public AdminService(ApplicationDBContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
        }


        public async Task<clsAdmin> GetByIDAsync(int ID) => await _dbContext.Set<clsAdmin>().FirstOrDefaultAsync(x => x.ID == ID);

        public async Task<clsAdmin> GetByNameAsync(string Name) => await _dbContext.Set<clsAdmin>().FirstOrDefaultAsync(x => x.Name == Name);

        public async Task<clsAdmin> GetByEmailAsync(string Email) => await _dbContext.Set<clsAdmin>().FirstOrDefaultAsync(x => x.Email == Email);
        public async Task<bool> IsExist(string Email) => await _dbContext.Set<clsAdmin>().AnyAsync(x => x.Email == Email);
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute hash from the password
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }


        public async Task<bool> ActiveThisAdmin(clsAdmin admin)
        {

            if (admin != null)
            {
                if (admin.IsActive)
                    admin.IsActive = false;
                else
                    admin.IsActive = true;
                _dbContext.Update(admin);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }


        public async Task<List<clsAdmin>> SearchForAdmin(string Name) => await _dbContext.Set<clsAdmin>().Where(x => x.Name.Contains(Name)).ToListAsync();


    }
}
