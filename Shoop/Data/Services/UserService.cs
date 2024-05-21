using Microsoft.EntityFrameworkCore;
using Shoop.Data.Base;
using Shoop.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shoop.Data.Services
{
    public class UserService :BaseRepositry<clsUser>, IUserSerevice
    {
        private readonly ApplicationDBContext _dbContext;

        public UserService(ApplicationDBContext dbContext):base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<clsUser> GetByIDAsync(int ID)=> await _dbContext.Set<clsUser>().FirstOrDefaultAsync(x=>x.ID==ID);

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
        public Task<clsUser> GetByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> IsExist(string Email)=>await _dbContext.Set<clsUser>().AnyAsync(x => x.Email == Email);

        public async Task<clsUser> GetByEmailAsync(string Email) => await _dbContext.Set<clsUser>().FirstOrDefaultAsync(x => x.Email == Email);

        public async Task<bool> ActiveThisUser(clsUser user)
        {
          
            if (user != null)
            {
                if(user.IsActive)
                user.IsActive = false;
                else
                    user.IsActive = true;
                _dbContext.Update(user);
                if (await _dbContext.SaveChangesAsync() > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> ChangePassword(clsUser user, string NewPassword)
        {
            user.Password = HashPassword(NewPassword);
          _dbContext.Update(user);

          if (await _dbContext.SaveChangesAsync() > 0)
            return true;
          else
                return false;
        }

        public async Task<List<clsUser>> SearchForUsers(string Name)=> await _dbContext.Set<clsUser>().Where(x=>x.Name.Contains(Name)).ToListAsync();
         
        public async Task<bool> IncreaseNumberOfPurshees(int UserID)
        {
            var user= await GetByIDAsync(UserID);

            user.Purchasesnumber++;
            _dbContext.Update(user);
            return await _dbContext.SaveChangesAsync()>0;
        }

    }
}
